using UnityEngine;
using Rive;
using UnityEngine.Rendering;
using Pwe.Core;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Pwe
{

    public class RiveTexture : MonoBehaviour
    {


        public event RiveEventDelegate OnRiveEvent;
        public delegate void RiveEventDelegate(ReportedEvent reportedEvent);

        [SerializeField] RenderTexture renderTexture;

        Fit fit = Fit.Layout;
        Alignment alignment = Alignment.Center;

        private CommandBuffer m_commandBuffer;

        Rive.Renderer m_riveRenderer;
        private Rive.File m_file;
        private Artboard m_artboard;
        private StateMachine m_stateMachine;

        Camera m_camera;
       // System.Action OnReady;
        bool isOn;

        // public void Init(string riveFileName, System.Action OnReady = null)
        public void Init(Rive.Asset asset)
        {
        //    this.OnReady = OnReady;
        //    MainApp.Instance.riveFilesManager.Load(riveFileName, OnDone);
        //}
        
        //void OnDone(byte[] data, string riveName)
        //{
            isOn = true;
           // m_file = Rive.File.Load(data, data.GetHashCode());
            m_file = Rive.File.Load(asset);

            MeshRenderer cubeRenderer = GetComponent<MeshRenderer>();
            Material mat = cubeRenderer.material;
            mat.mainTexture = renderTexture;


            if (!FlipY())
            {
                // Flip the render texture vertically for OpenGL
                mat.mainTextureScale = new Vector2(1, -1);
                mat.mainTextureOffset = new Vector2(0, 1);
            }

            Rive.RenderQueue m_renderQueue = new Rive.RenderQueue(renderTexture);
            m_riveRenderer = m_renderQueue.Renderer();

            //if (OnReady != null)
            //{
            //    OnReady();
            //    OnReady = null;
            //}           
        }
        public void InitArtboard()
        {

        }
        //IEnumerator Delayed()
        //{
        //    AddArtboard("intro");
        //    yield return new WaitForSeconds(2);
        //    AddArtboard("transition");
        //    yield return new WaitForSeconds(1);
        //    SetTrigger("transition");
        //    yield return new WaitForSeconds(1);
        //    m_stateMachine = stateMachines[0];
        //    SetTrigger("next");
        //}
        Dictionary<string, ArtboardData> artboardsData;
        class ArtboardData
        {
            public Artboard artboard;
            public Rive.StateMachine stateMachine;
        }
        private void AddData(string artName, Artboard artboard, StateMachine sm)
        {
            if (artboardsData.ContainsKey(artName))
                return;
            ArtboardData ad = new ArtboardData();
            ad.artboard = artboard;
            ad.stateMachine = sm;
            artboardsData.Add(artName, ad);
            print("New artboard: " + artName);
        }

        List<Artboard> artboards = new List<Artboard>();
        List<Rive.StateMachine> stateMachines = new List<Rive.StateMachine>();

        public void AddArtBoards(List<string> artboardsName)
        {
            artboardsData = new Dictionary<string, ArtboardData>();
            foreach (string artName in artboardsName)
            {
                m_artboard = m_file.Artboard(artName);
                m_stateMachine = m_artboard?.StateMachine();

                AddData(artName, m_artboard, m_stateMachine);
            }
        }
        string activeArtboard;
        public void ActivateArtboard(string artName)
        {
            if (activeArtboard == artName) return;
            activeArtboard = artName;
            if (!artboardsData.ContainsKey(artName))
            {
                Debug.LogError("No artboard nameed: " + artName + " loaded yet!");
                return;
            }
            Debug.Log("Activating artboard " + artName);
            ArtboardData ad = artboardsData[artName];
            m_artboard = m_file.Artboard(artName);
            m_stateMachine = m_artboard?.StateMachine();

            m_riveRenderer.Align(fit, alignment, m_artboard);
            m_riveRenderer.Draw(m_artboard);

            if (m_commandBuffer != null)
                m_camera.RemoveCommandBuffer(CameraEvent.AfterEverything, m_commandBuffer);

            m_commandBuffer = m_riveRenderer.ToCommandBuffer();
           
            m_commandBuffer.SetRenderTarget(renderTexture);
            m_commandBuffer.ClearRenderTarget(true, true, UnityEngine.Color.clear, 0.0f);
            m_riveRenderer.AddToCommandBuffer(m_commandBuffer);
            m_camera = Camera.main;
            m_camera.AddCommandBuffer(CameraEvent.AfterEverything, m_commandBuffer);
        }
        public void SetTrigger(string artboardName, string triggerName)
        {
            if (!isOn) return;
            ActivateArtboard(artboardName);
            print("Artboard: " + artboardName + " Set trigger: " + triggerName);

            SMITrigger someTrigger = m_stateMachine.GetTrigger(triggerName);
            if (someTrigger != null)
            {
                print("Set TRIGGER done! : " + triggerName);
                someTrigger.Fire();
            }
        }
        public void SetNumber(string artboardName, string triggerName, int number)
        {
            if (!isOn) return;
            ActivateArtboard(artboardName);
            print("Artboard: " + artboardName + " Set Number: " + triggerName + " num: " + number);
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
            if (m_commandBuffer != null && m_camera != null)
            {
                m_camera.RemoveCommandBuffer(CameraEvent.AfterEverything, m_commandBuffer);
            }

        }

        void OnDestroy()
        {
            if (renderTexture != null)
                renderTexture.Release();

            print("____________________OnDestroy");
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

            if (m_camera == null || renderTexture == null || m_artboard == null) return;


            if (!Physics.Raycast(m_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
                return;

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
        private static bool FlipY()
        {
#if UNITY_EDITOR
            return true;
#endif
#if UNITY_ANDROID
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
        public void SetNumberInArtboard(string nestedArtboardName, string triggerName, int value)
        {
            print("SetNumberInArtboard " + nestedArtboardName + " - trigger " + triggerName);
            m_artboard.SetNumberInputStateAtPath(triggerName, value, nestedArtboardName);
        }
        public void SetTriggerInArtboard(string nestedArtboardName, string triggerName)
        {
            print("SetTriggerInArtboard " + nestedArtboardName + " - trigger " + triggerName);
            m_artboard.FireInputStateAtPath(triggerName, nestedArtboardName);           
        }
    }
}