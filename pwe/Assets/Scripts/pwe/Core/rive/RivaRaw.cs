using Rive;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;


/// <summary>
/// Displays a Rive animation on a RawImage in Canvas and handles pointer events.
/// </summary>
[RequireComponent(typeof(RawImage))]
public class RivaRaw : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Tooltip("The Rive asset to draw.")]
    [SerializeField]
    private Rive.Asset asset;

    [Tooltip("Fit determines how the Rive content will be fitted to the view")]
    [SerializeField]
    private Fit fit = Fit.Fill;

    [Tooltip("Alignment determines how the content aligns with respect to the view bounds.")]
    private Alignment alignment = Alignment.Center;

    [Tooltip("The RawImage to display the Rive content.")]
    [SerializeField]
    private RawImage displayImage;

    [Tooltip("Sets the width and height of the RenderTexture for the Rive animation. Increasing this value improves the resolution but may impact performance.")]
    [SerializeField]
    private int size = 1024;

    //[Tooltip("The InputAction to track the pointer position.")]
    //[SerializeField]
    //InputActionReference pointerAction;


    private RenderTexture _renderTexture;
    private Rive.RenderQueue m_renderQueue;
    private Rive.Renderer m_riveRenderer;

    private File m_file;
    private Artboard m_artboard;
    private Rive.StateMachine m_stateMachine;

    private CommandBuffer m_commandBuffer;

    private void Awake()
    {
        if (displayImage == null)
        {
            displayImage = GetComponent<RawImage>();
        }

        _renderTexture = new RenderTexture(
            size,
            size,
            0,
            RenderTextureFormat.ARGB32
        );

       // _renderTexture.antiAliasing = QualitySettings.antiAliasing;

        if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D11)
        {
            // This is required for D3D11.
            _renderTexture.enableRandomWrite = true;
        }
        displayImage.texture = _renderTexture;

        m_renderQueue = new Rive.RenderQueue(_renderTexture);
        m_riveRenderer = m_renderQueue.Renderer();

        if (asset != null)
        {
            m_file = Rive.File.Load(asset);
            m_artboard = m_file.Artboard(0);
            m_stateMachine = m_artboard?.StateMachine();
        }


        if (m_artboard != null && _renderTexture != null)
        {
            m_riveRenderer.Align(fit, Alignment.Center, m_artboard);
            m_riveRenderer.Draw(m_artboard);

            m_commandBuffer = new CommandBuffer();
            m_commandBuffer.SetRenderTarget(_renderTexture);
            m_commandBuffer.ClearRenderTarget(true, true, UnityEngine.Color.clear, 0.0f);
            m_riveRenderer.AddToCommandBuffer(m_commandBuffer);
        }

        FlipTexture();
    }
    void OnDone(byte[] data, string riveName)
    {

    }


    private void OnEnable()
    {
        if (displayImage == null)
        {

            Debug.LogError("Display Image is not set.");
        }
      //  pointerAction.action.performed += OnPointerMove;
    }

    //private void OnDisable()
    //{
    //    pointerAction.action.performed -= OnPointerMove;
    //}

    //private void OnPointerMove(InputAction.CallbackContext context)
    //{
    //    Vector2 pointerValue = context.ReadValue<Vector2>();

    //    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(displayImage.rectTransform, pointerValue, null, out Vector2 localPoint))
    //    {

    //        Vector2 normalizedPoint = Rect.PointToNormalized(displayImage.rectTransform.rect, localPoint);
    //        HandlePointerMovement(normalizedPoint);
    //    }
    //}

    private void FlipTexture()
    {
        // Check if the texture needs to be flipped based on the graphics API
        if (ShouldFlipTexture())
        {
            displayImage.uvRect = new Rect(0, 1, 1, -1); // Flip the texture by adjusting the UV Rect
        }
    }

    /// <summary>
    /// Determines if the texture should be flipped based on the graphics API.
    /// </summary>
    /// <returns></returns>
    private static bool ShouldFlipTexture()
    {
        switch (SystemInfo.graphicsDeviceType)
        {
            case GraphicsDeviceType.Metal:
            case GraphicsDeviceType.Direct3D11:
                return true;
            default:
                return false;
        }
    }

    private void Update()
    {
        m_riveRenderer.Submit();

        if (m_stateMachine != null)
        {
            m_stateMachine.Advance(Time.deltaTime);

        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // We convert the pointer position to a local point within the RawImage
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(displayImage.rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
        {

            // Then normalize the local point to a [0, 1] range based on the RawImage size
            Vector2 normalizedPoint = Rect.PointToNormalized(displayImage.rectTransform.rect, localPoint);
            HandleInteraction(normalizedPoint, true);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(displayImage.rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
        {
            Vector2 normalizedPoint = Rect.PointToNormalized(displayImage.rectTransform.rect, localPoint);
            HandleInteraction(normalizedPoint, false); // false for pointer up
        }
    }

    /// <summary>
    /// Flips the normalized point if needed based on the graphics API. If we don't do this, the mouse interaction will be inverted.
    /// </summary>
    /// <param name="normalizedPoint"></param>
    /// <returns></returns>
    private Vector2 FlipNormalizedPointIfNeeded(Vector2 normalizedPoint)
    {
        if (ShouldFlipTexture())
        {
            normalizedPoint.y = 1 - normalizedPoint.y;
        }

        return normalizedPoint;
    }

    /// <summary>
    /// Passes the pointer down or up event to the Rive state machine.
    /// </summary>
    /// <param name="normalizedPoint"></param>
    /// <param name="isDown"></param>
    private void HandleInteraction(Vector2 normalizedPoint, bool isDown)
    {
        normalizedPoint = FlipNormalizedPointIfNeeded(normalizedPoint);
        var rect = new Rect(0, 0, _renderTexture.width, _renderTexture.height);
        Vector2 rivePoint = m_artboard.LocalCoordinate(new Vector2(normalizedPoint.x * rect.width, normalizedPoint.y * rect.height), rect, fit, alignment);

        if (isDown)
        {
            m_stateMachine?.PointerDown(rivePoint);
        }
        else
        {
            m_stateMachine?.PointerUp(rivePoint);
        }
    }


    /// <summary>
    /// Passes the pointer movement event to the Rive state machine.
    /// </summary>
    /// <param name="normalizedPoint"></param>
    private void HandlePointerMovement(Vector2 normalizedPoint)
    {
        normalizedPoint = FlipNormalizedPointIfNeeded(normalizedPoint);
        var rect = new Rect(0, 0, _renderTexture.width, _renderTexture.height);


        var screenPosition = new Vector2(normalizedPoint.x * rect.width, normalizedPoint.y * rect.height);
        Vector2 rivePoint = m_artboard.LocalCoordinate(screenPosition, rect, fit, alignment);
        m_stateMachine?.PointerMove(rivePoint);


    }

}