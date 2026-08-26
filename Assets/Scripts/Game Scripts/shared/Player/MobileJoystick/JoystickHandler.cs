using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("Joystick UI")]
    public RectTransform background;
    public RectTransform handle;

    [Header("Settings")]
    public float handleRange = 50f;

    private Vector2 _inputDirection = Vector2.zero;
    private Canvas _canvas;

    private void Start()
    {
        _canvas = GetComponentInParent<Canvas>();
        Debug.Log("JoystickHandler Start Canvas: " + _canvas);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Camera cam = _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background,
            eventData.position,
            cam,
            out Vector2 localPoint
        );

        float radius = background.sizeDelta.x * 0.5f;
        Vector2 clampedPoint = Vector2.ClampMagnitude(localPoint, radius);
        handle.localPosition = clampedPoint;

        _inputDirection = clampedPoint / radius;
        MobileInputProvider.Instance?.SetMoveInput(_inputDirection);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _inputDirection = Vector2.zero;
        handle.localPosition = Vector2.zero;
        MobileInputProvider.Instance?.SetMoveInput(Vector2.zero);
    }
}