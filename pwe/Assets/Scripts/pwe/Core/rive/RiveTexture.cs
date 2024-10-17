using UnityEngine;
using Rive;
using UnityEngine.Rendering;
using System.Linq;
using Pwe.Core;

namespace Pwe
{

    public class RiveTexture : MonoBehaviour
    {
        public Rive.Asset asset;
        public RenderTexture renderTexture;
        public Fit fit = Fit.contain;
        public Alignment alignment = Alignment.Center;


        private Rive.RenderQueue m_renderQueue;
        private Rive.Renderer m_riveRenderer;
        private CommandBuffer m_commandBuffer;

        private Rive.File m_file;
        private Artboard m_artboard;
        private StateMachine m_stateMachine;

        private Camera m_camera;
        System.Action OnReady;

        void OnDone(byte[] data, string riveName)
        {
            m_file = Rive.File.Load(riveName, data, data.GetHashCode());

            m_renderQueue = new Rive.RenderQueue(renderTexture);
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

                m_commandBuffer = m_riveRenderer.ToCommandBuffer();
                m_commandBuffer.SetRenderTarget(renderTexture);
                m_commandBuffer.ClearRenderTarget(true, true, UnityEngine.Color.clear, 0.0f);
                m_riveRenderer.AddToCommandBuffer(m_commandBuffer);
                m_camera = Camera.main;
                if (m_camera != null)
                {
                    Camera.main.AddCommandBuffer(CameraEvent.AfterEverything, m_commandBuffer);
                }
            }
            renderTexture.enableRandomWrite = true;
            if(OnReady != null)
                OnReady();
        }
        public void Init(string riveFileName, System.Action OnReady = null)
        {
            this.OnReady = OnReady;
            Vector3 s = transform.localScale;
            s.y *= -1;
            transform.localScale = s;

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
        }

        private void OnDisable()
        {
            if (m_camera != null && m_commandBuffer != null)
            {
                m_camera.RemoveCommandBuffer(CameraEvent.AfterEverything, m_commandBuffer);
            }
        }




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