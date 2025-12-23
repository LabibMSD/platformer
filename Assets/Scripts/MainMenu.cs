using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [Header("UI Components")]
    public Button[] buttons;

    [Header("Buttons Sounds")]
    public AudioSource buttonRollover;
    public AudioSource buttonClick;

    [Header("Modern UI Settings")]
    public float animationSpeed = 15f;
    public float hoverScale = 1.1f;
    public float clickScale = 0.95f;
    public bool useStaggeredEntrance = true;

    void Start()
    {
        SetupModernButtons();

        if (useStaggeredEntrance)
        {
            StartCoroutine(PlayEntranceAnimation());
        }
    }

    private void SetupModernButtons()
    {
        if (buttons == null || buttons.Length == 0)
            buttons = transform.GetComponentsInChildren<Button>();

        foreach (var btn in buttons)
        {
            if (btn == null) continue;
            
            ModernMenuButton modernBtn = btn.gameObject.GetComponent<ModernMenuButton>();
            if (modernBtn == null)
            {
                modernBtn = btn.gameObject.AddComponent<ModernMenuButton>();
            }
            modernBtn.Initialize(buttonRollover, buttonClick, hoverScale, clickScale, animationSpeed);
        }
    }

    private IEnumerator PlayEntranceAnimation()
    {
        foreach (var btn in buttons)
        {
            if(btn != null) btn.transform.localScale = Vector3.zero;
        }

        foreach (var btn in buttons)
        {
            if (btn != null)
            {
                StartCoroutine(AnimateScale(btn.transform, Vector3.one));
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public IEnumerator AnimateScale(Transform target, Vector3 endScale)
    {
        Vector3 startScale = target.localScale;
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * animationSpeed;
            target.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }
        target.localScale = endScale;
    }

    public void PlayHoverSound()
    {
        if (buttonRollover != null) buttonRollover.Play();
    }

    public void PlayClickSound()
    {
        if (buttonClick != null) buttonClick.Play();
    }

    public void LevelSelect(int level)
    {
        PlayClickSound();
        StartCoroutine(DelayedLoadScene(level));
    }

    public void OptionsButton()
    {
        PlayClickSound();
        PlayerPrefs.SetString("previous-scene", SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("Options");
    }

    public void ExitButton()
    {
        PlayClickSound();
        Debug.Log("Exited");
        Application.Quit();
    }

    IEnumerator DelayedLoadScene(int level)
    {
        yield return new WaitForSeconds(0.15f);
        SceneManager.LoadScene(level);
    }
}
