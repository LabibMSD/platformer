using UnityEngine;
using UnityEngine.EventSystems;

public class MobileButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum ButtonType { Jump, Sprint }
    public ButtonType Type;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (MobileInput.Instance == null) return;

        if (Type == ButtonType.Jump)
            MobileInput.Instance.PressJump();
        else if (Type == ButtonType.Sprint)
            MobileInput.Instance.Sprint = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (MobileInput.Instance == null) return;

        if (Type == ButtonType.Sprint)
            MobileInput.Instance.Sprint = false;
    }
}
