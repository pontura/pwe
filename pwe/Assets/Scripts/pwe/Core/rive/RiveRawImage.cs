using UnityEngine;
using Rive;
using UnityEngine.Rendering;
using System.Linq;
using Pwe.Core;
using UnityEngine.UI;

namespace Pwe
{
    public class RiveRawImage : MonoBehaviour
    {
        [SerializeField] Vector2 size = new Vector2(256, 256);
        public RenderTexture _renderTexture;
        public RawImage image;
        public Fit fit = Fit.Contain;
        public Alignment alignment = Alignment.Center;

        private Rive.RenderQueue m_renderQueue;
        private Rive.Renderer m_riveRenderer;
        private CommandBuffer m_commandBuffer;

        private Rive.File m_file;
        private Artboard m_artboard;
        private StateMachine m_stateMachine;

        System.Action OnReady;
        private Camera m_camera;

        public void Init(string riveFileName, System.Action OnReady = null)
        {
            this.OnReady = OnReady;
           // Vector2 s = image.transform.localScale;
           //// s.y = Mathf.Abs(image.transform.localScale.y)*-1;
           // image.transform.localScale = s;
            MainApp.Instance.riveFilesManager.Load(riveFileName, OnDone);
            SendMessage("Rescaled",SendMessageOptions.DontRequireReceiver); // For ResolutionFixer
        }
        void OnDone(byte[] data, string riveName)
        {
            m_file = Rive.File.Load(data, data.GetHashCode());

            RenderTexture renderTexture = new RenderTexture(
                (int)size.x,
                (int)size.y, 
                0,
                RenderTextureFormat.ARGB32);

            renderTexture.enableRandomWrite = true;

            renderTexture.Create();

            m_renderQueue = new Rive.RenderQueue(renderTexture);

            m_riveRenderer = m_renderQueue.Renderer();

            m_artboard = m_file.Artboard(0);
            m_stateMachine = m_artboard?.StateMachine();
            //}
            if (m_artboard != null && renderTexture != null)
            {
                m_riveRenderer.Align(fit, alignment, m_artboard);
                m_riveRenderer.Draw(m_artboard);

                m_commandBuffer = new CommandBuffer();
                m_commandBuffer.SetRenderTarget(renderTexture);
                m_commandBuffer.ClearRenderTarget(true, true, UnityEngine.Color.clear, 0.0f);
                m_riveRenderer.AddToCommandBuffer(m_commandBuffer);
            }
            renderTexture.enableRandomWrite = true;
            image.texture = renderTexture;

            if (!FlipY())
            {
                print("rotame: " + gameObject.name);
                // Flip the render texture vertically for OpenGL
                //image.material.mainTextureScale = new Vector2(1, -1);
                //image.material.mainTextureOffset = new Vector2(0, 1);
                image.transform.localEulerAngles = new Vector3(0, 180, 180);
            }

            if (OnReady != null)
                OnReady();
            Invoke("Invoked", 0.5f);
        }
        private static bool FlipY()
        {
#if UNITY_EDITOR
            return true;
#endif
            switch (UnityEngine.SystemInfo.graphicsDeviceType)
            {
                case UnityEngine.Rendering.GraphicsDeviceType.Metal:
                case UnityEngine.Rendering.GraphicsDeviceType.Direct3D11:
                    return true;
                default:
                    return false;
            }
        }
        private void Invoked()
        {
            if (m_camera != null && m_commandBuffer != null)
            {
                m_camera.RemoveCommandBuffer(CameraEvent.AfterEverything, m_commandBuffer);
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
            print("SetNumber : " + triggerName + " num: " + number);
            if (number < 0) return;
            SMINumber someNumber = m_stateMachine.GetNumber(triggerName);
            if (someNumber == null) return;
            someNumber.Value = number;
            print("SetNumber Done");

        }
        private void Update()
        {
            if (m_stateMachine != null)
            {
                m_stateMachine.Advance(Time.deltaTime);
            }
            if (m_riveRenderer != null)
            {
                m_riveRenderer.Submit();
                GL.InvalidateState();
            }
            // m_riveRenderer.Submit();

           
        }

        //private void OnDisable()
        //{
        //    if (m_camera != null && m_commandBuffer != null)
        //    {
        //        m_camera.RemoveCommandBuffer(CameraEvent.AfterEverything, m_commandBuffer);
        //    }
        //}




        public void PlayStateMachine(string stateMachine, string triggerName)
        {
            print("PlayStateMachine: " + stateMachine + " trigger: " + triggerName);

            SMITrigger someTrigger = m_stateMachine.GetTrigger(triggerName);
            if (someTrigger != null)
            {
                print("TRIGGER : " + triggerName);
                someTrigger.Fire();
            }
        }
    }
}