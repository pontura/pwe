using UnityEngine;
using Rive;
using UnityEngine.Rendering;
using System.Linq;
using Pwe.Core;
using UnityEngine.UI;
using System.Collections.Generic;
using NumericsConverter;

namespace Pwe
{
    public class MultiRiveRawImage : MonoBehaviour
    {
        [SerializeField] Vector2 size = new Vector2(256, 256);
        public RenderTexture _renderTexture;
        public RawImage image;
        public Fit fit = Fit.Contain;
        public Alignment alignment = Alignment.Center;

        private Rive.RenderQueue m_renderQueue;
        private Rive.Renderer m_riveRenderer;
        private CommandBuffer m_commandBuffer;

        [SerializeField] List<StateMachine> m_stateMachines;

        System.Action<int> OnReady;
        private Camera m_camera;


        public void Init() {
            //size = new Vector2(Screen.width, Screen.height);
            //((RectTransform)transform).sizeDelta = new Vector2(Screen.width, Screen.height);

            m_stateMachines = new();

            if (_renderTexture == null) {
                _renderTexture = new RenderTexture(
                    (int)size.x,
                    (int)size.y,
                    0,
                    RenderTextureFormat.ARGB32);

                if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D11) {
                    _renderTexture.enableRandomWrite = true;
                }
                _renderTexture.antiAliasing = 1;

                _renderTexture.Create();
            }            

            m_renderQueue = new Rive.RenderQueue(_renderTexture);

            m_riveRenderer = m_renderQueue.Renderer();

            image.texture = _renderTexture;

            m_commandBuffer = new CommandBuffer();
            m_commandBuffer.SetRenderTarget(_renderTexture);
            // m_commandBuffer.ClearRenderTarget(true, true, UnityEngine.Color.clear, 0.0f);
            m_riveRenderer.AddToCommandBuffer(m_commandBuffer);

            // Vector2 s = image.transform.localScale;
            //// s.y = Mathf.Abs(image.transform.localScale.y)*-1;
            // image.transform.localScale = s;
            // For ResolutionFixer
        }

        public void LoadArtboard(string riveFileName, int arboardIndex = 0, Transform t = null, System.Action<int> OnReady = null) {
            this.OnReady = OnReady;
            MainApp.Instance.riveFilesManager.Load(riveFileName, (bytes, filename)=> {
                Rive.File m_file = Rive.File.Load(bytes, bytes.GetHashCode());
                Artboard artboard = m_file.Artboard((uint)arboardIndex);
                AddArtboard(artboard, t);
            });
            SendMessage("Rescaled", SendMessageOptions.DontRequireReceiver);
        }

        public void AddArtboard(Artboard artboard, Transform t=null) {
            //m_file = Rive.File.Load(data, data.GetHashCode());
            //artboard = m_file.Artboard(0);

            StateMachine stateMachine = artboard?.StateMachine();
            if (stateMachine != null)
                m_stateMachines.Add(stateMachine);
            //}

            m_riveRenderer.Align(fit, alignment, artboard);
            float scaleFactor = 0.1f;
            float scaleVal = _renderTexture.width * scaleFactor / artboard.Width;
            System.Numerics.Matrix3x2 scale = System.Numerics.Matrix3x2.CreateScale(scaleVal);
            System.Numerics.Matrix3x2 invScale = System.Numerics.Matrix3x2.CreateScale(1f/ scaleVal);

            /*System.Numerics.Matrix3x2 m3 = System.Numerics.Matrix3x2.Identity;
            if (t != null) {
                System.Numerics.Matrix4x4 m4 = t.localToWorldMatrix.ToSystem();
                m3 = new System.Numerics.Matrix3x2(m4.M11, m4.M12, m4.M21, m4.M22, m4.M31, m4.M32);
            }*/

            m_riveRenderer.Transform(scale);
            /*Debug.Log("% Position: " + t.position);
            Debug.Log("% Screen: " + Screen.width+"x"+Screen.height);            
            Vector2 newPos = ((Vector2)(t.position) - new Vector2(Screen.width * 0.5f, Screen.height * 0.5f));
            Debug.Log("% New Pos: " + newPos);*/
            //t.TransformPoint
            Vector2 translate = new Vector2(_renderTexture.width * -0.5f, _renderTexture.height-artboard.Height);
            m_riveRenderer.Translate(translate.ToSystem());
            //m_riveRenderer.Translate(new System.Numerics.Vector2(-100,0));
            m_riveRenderer.Draw(artboard);

            //m_riveRenderer.Translate(-1*translate.ToSystem());
            m_riveRenderer.Transform(invScale);

            if (!FlipY())
            {
                print("rotame: " + gameObject.name);
                // Flip the render texture vertically for OpenGL
                //image.material.mainTextureScale = new Vector2(1, -1);
                //image.material.mainTextureOffset = new Vector2(0, 1);
                image.transform.localEulerAngles = new Vector3(0, 180, 180);
            }

            if (OnReady != null)
                OnReady(m_stateMachines.Count-1);           
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
        public void SetTrigger(int stateMachineIndex, string triggerName)
        {
            print("Set trigger: " + triggerName);

            SMITrigger someTrigger = m_stateMachines[stateMachineIndex].GetTrigger(triggerName);
            if (someTrigger != null)
            {
                print("Set TRIGGER done! : " + triggerName);
                someTrigger.Fire();
            }
        }
        public void SetNumber(int stateMachineIndex, string triggerName, int number)
        {
            print("SetNumber : " + triggerName + " num: " + number);
            if (number < 0) return;
            SMINumber someNumber = m_stateMachines[stateMachineIndex].GetNumber(triggerName);
            if (someNumber == null) return;
            someNumber.Value = number;
            print("SetNumber Done");

        }
        private void Update()
        {
            foreach(StateMachine sm in m_stateMachines)
            if (sm != null)
            {
                sm.Advance(Time.deltaTime);
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




        public void PlayStateMachine(int stateMachineIndex, string stateMachine, string triggerName)
        {
            print("PlayStateMachine: " + stateMachine + " trigger: " + triggerName);

            SMITrigger someTrigger = m_stateMachines[stateMachineIndex].GetTrigger(triggerName);
            if (someTrigger != null)
            {
                print("TRIGGER : " + triggerName);
                someTrigger.Fire();
            }
        }
    }
}