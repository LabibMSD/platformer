using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ModernMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Selectable uiElement;
    private Coroutine currentAnim;

    // Settings assigned by the controller
    public AudioSource hoverSound;
    public AudioSource clickSound;
    public float hoverScale = 1.1f;
    public float clickScale = 0.95f;
    public float animationSpeed = 15f;
    public bool animateScale = true;

    private Vector3 originalScale;

    private void Awake()
    {
        uiElement = GetComponent<Selectable>();
        originalScale = transform.localScale;
    }

    public void Initialize(AudioSource hover, AudioSource click, float hScale, float cScale, float speed)
    {
        hoverSound = hover;
        clickSound = click;
        hoverScale = hScale;
        clickScale = cScale;
        animationSpeed = speed;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (uiElement != null && !uiElement.interactable) return;
        
        if (hoverSound != null) hoverSound.Play();
        
        if (animateScale)
        {
            if (currentAnim != null) StopCoroutine(currentAnim);
            currentAnim = StartCoroutine(AnimateScale(originalScale * hoverScale));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (uiElement != null && !uiElement.interactable) return;

        if (animateScale)
        {
            if (currentAnim != null) StopCoroutine(currentAnim);
            currentAnim = StartCoroutine(AnimateScale(originalScale));
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (uiElement != null && !uiElement.interactable) return;

        if (clickSound != null) clickSound.Play();

        if (animateScale)
        {
            if (currentAnim != null) StopCoroutine(currentAnim);
            currentAnim = StartCoroutine(AnimateScale(originalScale * clickScale));
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (uiElement != null && !uiElement.interactable) return;

        // Return to Hover Scale
        if (animateScale)
        {
            if (currentAnim != null) StopCoroutine(currentAnim);
            currentAnim = StartCoroutine(AnimateScale(originalScale * hoverScale));
        }
    }

    private IEnumerator AnimateScale(Vector3 endScale)
    {
        Vector3 startScale = transform.localScale;
        float t = 0;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * animationSpeed;
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }
        transform.localScale = endScale;
    }
}
