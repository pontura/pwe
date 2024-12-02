using UnityEngine;
using Rive;
using UnityEngine.Rendering;
using Pwe.Core;
using System.Linq;

namespace Pwe
{

    public class RiveTexture : MonoBehaviour
    {

        public event RiveEventDelegate OnRiveEvent;
        public delegate void RiveEventDelegate(ReportedEvent reportedEvent);

        //public RenderTexture renderTexture;
        [SerializeField] Vector2 size = new Vector2(256, 256);
        [SerializeField] RenderTexture renderTexture;
        public Fit fit = Fit.Contain;
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
            MainApp.Instance.riveFilesManager.Load(riveFileName, OnDone);
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
            m_file = Rive.File.Load(data, data.GetHashCode());

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
               // renderTexture.format = RenderTextureFormat.ARGB32;
                renderTexture.enableRandomWrite = true;
                renderTexture.Create();
            }

            MeshRenderer cubeRenderer = GetComponent<MeshRenderer>();
            Material mat = cubeRenderer.material;
            mat.mainTexture = renderTexture;



            //if (!FlipY())
            //{
            //    // Flip the render texture vertically for OpenGL
            //    mat.mainTextureScale = new Vector2(1, -1);
            //    mat.mainTextureOffset = new Vector2(0, 1);
            //}

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

                m_camera = Camera.main;

                if (m_camera != null)
                {
                    m_camera.AddCommandBuffer(CameraEvent.AfterEverything, m_commandBuffer);
                }
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
            HitTesting();
            // m_riveRenderer.Submit();
            foreach (var report in m_stateMachine?.ReportedEvents() ?? Enumerable.Empty<ReportedEvent>())
            {
                OnRiveEvent?.Invoke(report);
            }
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
        bool m_wasMouseDown = false;
        private Vector2 m_lastMousePosition;

        void HitTesting()
        {

          //  if (camera == null || renderTexture == null || m_artboard == null) return;


            if (!Physics.Raycast(m_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
                return;

            //Artboard bowls = m_file.Artboard("Bowls");
            //bowls.LocalCoordinate(new Vector2(200,500), new Rect(200, 0, Screen.width, Screen.height), fit, alignment);



            UnityEngine.Renderer rend = hit.transform.GetComponent<UnityEngine.Renderer>();
            MeshCollider meshCollider = hit.collider as MeshCollider;

            if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
                return;

            Vector2 pixelUV = hit.textureCoord;

            pixelUV.x *= renderTexture.width;
            pixelUV.y *= renderTexture.height;

            Vector3 mousePos = m_camera.ScreenToViewportPoint(Input.mousePosition);
            Vector2 mouseRiveScreenPos = new(mousePos.x * m_camera.pixelWidth, (1 - mousePos.y) * m_camera.pixelHeight);

            if (m_lastMousePosition != mouseRiveScreenPos || transform.hasChanged)
            {
                Vector2 local = m_artboard.LocalCoordinate(pixelUV, new Rect(0, 0, renderTexture.width, renderTexture.height), fit, alignment);
                m_stateMachine?.PointerMove(local);
                m_lastMousePosition = mouseRiveScreenPos;
            }
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 local = m_artboard.LocalCoordinate(pixelUV, new Rect(0, 0, renderTexture.width, renderTexture.height), fit, alignment);
                m_stateMachine?.PointerDown(local);
                m_wasMouseDown = true;
            }
            else if (m_wasMouseDown)
            {
                m_wasMouseDown = false; Vector2 local = m_artboard.LocalCoordinate(mouseRiveScreenPos, new Rect(0, 0, renderTexture.width, renderTexture.height), fit, alignment);
                m_stateMachine?.PointerUp(local);
            }
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