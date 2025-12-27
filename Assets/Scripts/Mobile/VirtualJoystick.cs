using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image bgImg;
    private Image joystickImg;
    private Vector3 inputVector;

    private void Start()
    {
        bgImg = GetComponent<Image>();
        
        if (transform.childCount > 0)
        {
            joystickImg = transform.GetChild(0).GetComponent<Image>();
        }
        else
        {
            Debug.LogError($"VirtualJoystick Error pada object '{gameObject.name}': Object ini tidak memiliki anak (Child) untuk Handle! Pastikan script 'VirtualJoystick' hanya terpasang di 'JoystickBG', dan 'JoystickBG' punya child 'JoystickHandle'.");
        }
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            // Calculate normalized position (0 to 1) relative to the background rect
            float normalizedX = (pos.x - bgImg.rectTransform.rect.x) / bgImg.rectTransform.rect.width;
            float normalizedY = (pos.y - bgImg.rectTransform.rect.y) / bgImg.rectTransform.rect.height;

            // Convert to -1 to 1 range
            inputVector = new Vector3(normalizedX * 2 - 1, 0, normalizedY * 2 - 1);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            // Move Joystick Handle
            if (joystickImg != null)
            {
                joystickImg.rectTransform.anchoredPosition = new Vector3(inputVector.x * (bgImg.rectTransform.sizeDelta.x / 2), inputVector.z * (bgImg.rectTransform.sizeDelta.y / 2));
            }

            // Update Singleton
            if (MobileInput.Instance != null)
            {
                MobileInput.Instance.Horizontal = inputVector.x;
                MobileInput.Instance.Vertical = inputVector.z;
            }
        }
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        inputVector = Vector3.zero;
        if (joystickImg != null)
        {
            joystickImg.rectTransform.anchoredPosition = Vector3.zero;
        }

        if (MobileInput.Instance != null)
        {
            MobileInput.Instance.Horizontal = 0;
            MobileInput.Instance.Vertical = 0;
        }
    }
}
