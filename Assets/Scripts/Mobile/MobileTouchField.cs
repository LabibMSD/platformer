using UnityEngine;
using UnityEngine.EventSystems;

public class MobileTouchField : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Vector2 lastPosition;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 delta = eventData.position - lastPosition;
        lastPosition = eventData.position;

        if (MobileInput.Instance != null)
        {
            MobileInput.Instance.LookX = delta.x * 0.1f; // Adjust sensitivity factor as needed
            MobileInput.Instance.LookY = delta.y * 0.1f;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        lastPosition = eventData.position;
        if (MobileInput.Instance != null)
        {
            MobileInput.Instance.IsTouchingField = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (MobileInput.Instance != null)
        {
            MobileInput.Instance.LookX = 0;
            MobileInput.Instance.LookY = 0;
            MobileInput.Instance.IsTouchingField = false;
        }
    }
}
