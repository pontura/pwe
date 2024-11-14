using UnityEngine;
using Rive;
using UnityEngine.Rendering;
using Pwe.Core;

namespace Pwe
{

    public class RiveTexture : MonoBehaviour
    {
        //public RenderTexture renderTexture;
        [SerializeField] Vector2 size = new Vector2(256, 256);
        [SerializeField] RenderTexture renderTexture;
        public Fit fit = Fit.contain;
        public Alignment alignment = Alignment.Center;

        private CommandBuffer m_commandBuffer;

        Rive.Renderer m_riveRenderer;
        private Rive.File m_file;
        private Artboard m_artboard;
        private StateMachine m_stateMachine;

        Camera m_camera;
        System.Action OnReady;
        bool isOn;

        public void Init(string riveFileName, System.Action OnReady = null)
        {
            this.OnReady = OnReady;
            //Vector2 s = transform.localScale;
            //s.y = Mathf.Abs(transform.localScale.y)*-1;
            //transform.localScale = s;

            // If on D3d11, this is required
            MainApp.Instance.riveFilesManager.Load(riveFileName, OnDone);
            //for (uint a = 0; a < m_file.ArtboardCount; a++)
            //{
            //    Artboard artb = m_file.Artboard(a);
            //    // Try using the GetName() method or similar alternatives
            //    for (uint b = 0; b < artb?.StateMachineCount; b++)
            //    {
            //       // StateMachine s = artb?.StateMachineName(b);
            //        Debug.Log(a + " StateMachine: " + artb?.StateMachineName(b));
            //    }
            //}
            //foreach (var reportedEvent in m_stateMachine?.ReportedEvents() ?? Enumerable.Empty<ReportedEvent>())
            //{
            //    Debug.Log($"Event received, name: \"{reportedEvent.Name}\", secondsDelay: {reportedEvent.SecondsDelay}");
            //}

            //// Important! Call `advance` after accessing events.
            //m_stateMachine?.Advance(Time.deltaTime);
        }
        private static bool FlipY()
        {
#if UNITY_EDITOR
            return false;
#endif
            return true; // para android se necesita FlipY

            //switch (UnityEngine.SystemInfo.graphicsDeviceType)
            //{
            //    case UnityEngine.Rendering.GraphicsDeviceType.Metal:
            //    case UnityEngine.Rendering.GraphicsDeviceType.Direct3D11:
            //        return true;
            //    default:
            //        return false;
            //}
        }
        void OnDone(byte[] data, string riveName)
        {
            isOn = true;
            m_file = Rive.File.Load(riveName, data, data.GetHashCode());

            print("renderTexture: " + renderTexture);

            if (renderTexture == null)
            {
                renderTexture = new RenderTexture(
                    (int)size.x,
                    (int)size.y,
                    0,
                    RenderTextureFormat.ARGB32);


                renderTexture.enableRandomWrite = true;

                //  renderTexture.antiAliasing = QualitySettings.antiAliasing;

                renderTexture.Create();
            }
            else
            {
                renderTexture.format = RenderTextureFormat.ARGB32;
                renderTexture.enableRandomWrite = true;
                renderTexture.Create();
            }

            MeshRenderer cubeRenderer = GetComponent<MeshRenderer>();
            Material mat = cubeRenderer.material;
            mat.mainTexture = renderTexture;

            if (!FlipY())
            {
                mat.mainTextureScale = new Vector2(1, -1);
                mat.mainTextureOffset = new Vector2(0, 1);
            }

            Rive.RenderQueue m_renderQueue = new Rive.RenderQueue(renderTexture);
            m_riveRenderer = m_renderQueue.Renderer();
            //if (asset != null)
            //{
            //    m_file = Rive.File.Load(asset);
            m_artboard = m_file.Artboard(0);
            m_stateMachine = m_artboard?.StateMachine();
            //}
            if (m_artboard != null && renderTexture != null)
            {
                m_riveRenderer.Align(fit, alignment, m_artboard);
                m_riveRenderer.Draw(m_artboard);

                //m_commandBuffer = m_riveRenderer.ToCommandBuffer();
                //m_commandBuffer.SetRenderTarget(renderTexture);
                //m_commandBuffer.ClearRenderTarget(true, true, UnityEngine.Color.clear, 0.0f);
                //m_riveRenderer.AddToCommandBuffer(m_commandBuffer);

                m_commandBuffer = new CommandBuffer();
                m_commandBuffer.SetRenderTarget(renderTexture);
                m_commandBuffer.ClearRenderTarget(true, true, UnityEngine.Color.clear, 0.0f);
                m_riveRenderer.AddToCommandBuffer(m_commandBuffer);

                //m_camera = Camera.main;

                //if (m_camera != null)
                //{
                //    m_camera.AddCommandBuffer(CameraEvent.AfterEverything, m_commandBuffer);
                //}
            }
            if (OnReady != null)
            {
                OnReady();
                OnReady = null;
            }
        }
        //StateMachine GetArtboard(string s)
        //{
        //    for (uint a = 0; a < m_file.ArtboardCount; a++)
        //    {
        //        Artboard artb = m_file.Artboard(a);
        //        // Try using the GetName() method or similar alternatives
        //        for (uint b = 0; b < artb?.StateMachineCount; b++)
        //        {
        //            string _s = artb?.StateMachineName(b);
        //            if (s == _s)
        //                return artb.StateMachine();
        //        }
        //    }
        //    return null;
        //}
        public void SetTrigger(string triggerName)
        {
            if (!isOn) return;
            print("Set trigger: " + triggerName);

            SMITrigger someTrigger = m_stateMachine.GetTrigger(triggerName);
            if (someTrigger != null)
            {
                print("Set TRIGGER done! : " + triggerName);
                someTrigger.Fire();
            }
        }
        public void SetNumber(string triggerName, int number)
        {
            if (!isOn) return;
            print("SetNumber : " + triggerName + " num: " + number);
            SMINumber someNumber = m_stateMachine.GetNumber(triggerName);
            if (someNumber == null) return;
            someNumber.Value = number;
            print("SetNumber Done");
        }
        private void Update()
        {
            if (!isOn) return;
            if (m_riveRenderer != null)
            {
                m_riveRenderer.Submit();
                GL.InvalidateState();
            }
           // m_riveRenderer.Submit();

            if (m_stateMachine != null)
            {
                m_stateMachine.Advance(Time.deltaTime);
            }
        }

        private void OnDisable()
        {
            isOn = false;           
        }

        void OnDestroy()
        {
            if (m_camera != null && m_commandBuffer != null)
            {
                m_camera.RemoveCommandBuffer(CameraEvent.AfterEverything, m_commandBuffer);
            }
            if (renderTexture != null)
                renderTexture.Release();

            print("____________________OnDestroy");
            Object.Destroy(renderTexture);
            m_camera = null;
            m_commandBuffer = null;
            m_riveRenderer = null;
            m_file = null;
            m_artboard = null;
            m_stateMachine = null;
            renderTexture = null;
        }


        public void PlayStateMachine(string stateMachine, string triggerName)
        {
            if (!isOn) return;
            SMITrigger someTrigger = m_stateMachine.GetTrigger(triggerName);
            if (someTrigger != null)
            {
                print("TRIGGER : " + triggerName);
                someTrigger.Fire();
            }
        }
    }
}