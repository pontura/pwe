// Attach this script to gameobject with RawImage, create a new material with GammaSpaceUI shader and reference it to the script.
// you can also change the artboard index if you have multiple artboard in same rive asset but its not recommended for low end devices as it causes heavy FPS drop 
//Any further optimization if always welcomed , if you do make changes in script for good do let me know I will update this.

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Rive;
using System.Collections;
namespace Pwe
{
    //[ExecuteAlways]
    public class RiveUITexture : MonoBehaviour
    {
        public RawImage uiImage;
        public Asset asset;
        public Fit fit = Fit.Contain;
        public Material UICompositeMaterial;

        [SerializeField]
        private uint _artboardIndex = 0;
        public uint artboardIndex
        {
            get => _artboardIndex;
            set
            {
                if (_artboardIndex != value)
                {
                    _artboardIndex = value;
                    if (Application.isPlaying)
                    {
                        ReinitializeArtboard();
                    }
                }
            }
        }

        public RenderTexture outputRenderTexture;

        private Rive.RenderQueue m_renderQueue;
        private Rive.Renderer m_riveRenderer;
        private CommandBuffer m_commandBuffer;

        private File m_file;
        private Artboard m_artboard;
        private StateMachine m_stateMachine;
        public StateMachine stateMachine => m_stateMachine;

        private int uitex_id = Shader.PropertyToID("_UITex");

        public void OnEnable()
        {
            InitializeRenderTexture();
            SetupRiveComponents();
        }

        private void OnDisable()
        {
            CleanupRiveComponents();
        }

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                StartCoroutine(DelayedReinitialize());
            }
        }

        private IEnumerator DelayedReinitialize()
        {
            yield return new WaitForEndOfFrame();
            ReinitializeArtboard();
        }

        private void InitializeRenderTexture()
        {
            if (outputRenderTexture == null || !outputRenderTexture.IsCreated())
            {
                if (outputRenderTexture != null)
                {
                    outputRenderTexture.Release();
                }

                float width = uiImage.rectTransform.rect.width;
                float height = uiImage.rectTransform.rect.height;
                outputRenderTexture = new RenderTexture((int)width, (int)height, 0, RenderTextureFormat.ARGB32);
                outputRenderTexture.enableRandomWrite = true;
                outputRenderTexture.Create();
            }

            if (uiImage != null)
            {
                uiImage.texture = outputRenderTexture;

                if (RiveFlipYOnGraphicsDevice())
                {
                    uiImage.uvRect = new Rect(0, 1, 1, -1);
                }
            }
        }

        private void SetupRiveComponents()
        {
            m_renderQueue = new Rive.RenderQueue(outputRenderTexture);
            m_riveRenderer = m_renderQueue.Renderer();

            if (asset != null)
            {
                m_file = Rive.File.Load(asset);
                InitializeArtboard();
            }
        }

        private void InitializeArtboard()
        {
            if (m_file != null)
            {
                m_artboard = m_file.Artboard(artboardIndex);
                m_stateMachine = m_artboard?.StateMachine();

                if (m_artboard != null)
                {
                    m_riveRenderer.Align(fit, new Alignment(0, 0), m_artboard);
                    m_riveRenderer.Draw(m_artboard);

                    if (m_commandBuffer == null)
                    {
                        m_commandBuffer = new CommandBuffer();
                    }
                    else
                    {
                        m_commandBuffer.Clear();
                    }

                    m_commandBuffer.SetRenderTarget(outputRenderTexture);
                    m_commandBuffer.ClearRenderTarget(true, true, UnityEngine.Color.clear, 0.0f);
                    m_riveRenderer.AddToCommandBuffer(m_commandBuffer);

                    Graphics.ExecuteCommandBuffer(m_commandBuffer);
                    m_renderQueue.UpdateTexture(outputRenderTexture);
                }
            }
        }

        public void ReinitializeArtboard()
        {
            CleanupRiveComponents();
            InitializeRenderTexture();
            SetupRiveComponents();
        }

        private void CleanupRiveComponents()
        {
            if (m_commandBuffer != null)
            {
                m_commandBuffer.Clear();
                m_commandBuffer.Release();
                m_commandBuffer = null;
            }

            m_stateMachine = null;
            m_artboard = null;
            m_file = null;
            m_renderQueue = null;
            m_riveRenderer = null;
        }

        private void LateUpdate()
        {
            if (m_riveRenderer != null && m_artboard != null)
            {
                m_riveRenderer.Draw(m_artboard);
                m_riveRenderer.Submit();
            }

            if (m_stateMachine != null)
            {
                m_stateMachine.Advance(Time.deltaTime);
            }

            if (m_commandBuffer != null &&
                (SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES2 ||
                 SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES3))
            {
                Graphics.ExecuteCommandBuffer(m_commandBuffer);
            }
#if !UNITY_EDITOR
        ApplyGammaCorrection();
#endif
        }

        private void ApplyGammaCorrection()
        {
            if (UICompositeMaterial != null)
            {
                RenderTexture tempRT = RenderTexture.GetTemporary(outputRenderTexture.width, outputRenderTexture.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);

                UICompositeMaterial.SetTexture(uitex_id, outputRenderTexture);

#if !UNITY_ANDROID && !UNITY_IOS
            GL.sRGBWrite = false;
#endif

                Graphics.Blit(outputRenderTexture, tempRT, UICompositeMaterial, 0);

#if !UNITY_ANDROID && !UNITY_IOS
            GL.sRGBWrite = true;
#endif

                Graphics.Blit(tempRT, outputRenderTexture);

                RenderTexture.ReleaseTemporary(tempRT);
            }
        }

        private void OnDestroy()
        {
            CleanupRiveComponents();
            if (outputRenderTexture != null)
            {
                outputRenderTexture.Release();
                outputRenderTexture = null;
            }
        }

        public static bool RiveFlipYOnGraphicsDevice()
        {
            return SystemInfo.graphicsDeviceType == GraphicsDeviceType.Metal ||
                   SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D11;
        }
    }
}