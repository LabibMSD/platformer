using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    private Toggle invertYAxis;
    public Slider BGMSlider;
    public Slider SFXSlider;
    private float initialVolumeBGM;
    private float initialVolumeSFX;

    [Header("Modern UI Settings")]
    public AudioSource buttonRollover;
    public AudioSource buttonClick;
    public float animationSpeed = 15f;
    public float hoverScale = 1.05f; // Smaller scale for options components
    public float clickScale = 0.98f;

    void Start()
    {
        // Original Logic
        Button applyBtn = transform.Find("ApplyButton").gameObject.GetComponent<Button>();
        applyBtn.onClick.AddListener(Apply);
        
        invertYAxis = transform.Find("InvertYToggle").gameObject.GetComponent<Toggle>();
        if (PlayerPrefs.HasKey("InvertYToggle"))
            invertYAxis.isOn = PlayerPrefs.GetInt("InvertYToggle") == 0 ? false : true;
        
        SetBGMSlider();
        SetSFXSlider();

        // Modern UI Setup
        SetupModernButtons();
        StartCoroutine(PlayEntranceAnimation());
    }

    void SetupModernButtons()
    {
        // Find all interactive elements (Buttons, Toggles, Sliders)
        Selectable[] selectables = GetComponentsInChildren<Selectable>();

        foreach (var sel in selectables)
        {
            if (sel == null) continue;
            
            ModernMenuButton modernBtn = sel.gameObject.GetComponent<ModernMenuButton>();
            if (modernBtn == null)
            {
                modernBtn = sel.gameObject.AddComponent<ModernMenuButton>();
            }
            modernBtn.Initialize(buttonRollover, buttonClick, hoverScale, clickScale, animationSpeed);
        }
    }

    IEnumerator PlayEntranceAnimation()
    {
        Selectable[] selectables = GetComponentsInChildren<Selectable>();
        foreach (var sel in selectables)
        {
            sel.transform.localScale = Vector3.zero;
        }

        foreach (var sel in selectables)
        {
            if (sel != null)
            {
                StartCoroutine(AnimateScale(sel.transform, Vector3.one));
                yield return new WaitForSeconds(0.05f); // Faster stagger for options
            }
        }
    }

    IEnumerator AnimateScale(Transform target, Vector3 endScale)
    {
        Vector3 startScale = target.localScale;
        float t = 0;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * animationSpeed;
            target.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }
        target.localScale = endScale;
    }

    public void Back()
    {
        SetBGMVolume(initialVolumeBGM);
        
        if (buttonClick != null) buttonClick.Play();

        if (PlayerPrefs.HasKey("previous-scene"))
            SceneManager.LoadScene(PlayerPrefs.GetString("previous-scene"));        
    }

    public void Apply()
    {
        if (buttonClick != null) buttonClick.Play();

        if (invertYAxis.isOn)
            PlayerPrefs.SetInt("InvertYToggle", 1);
        else
            PlayerPrefs.SetInt("InvertYToggle", 0);
            
        PlayerPrefs.SetFloat("dbBGMVolume", LinearToDecibel(BGMSlider.value));
        PlayerPrefs.SetFloat("dbSFXVolume", LinearToDecibel(SFXSlider.value));
        
        if (PlayerPrefs.HasKey("previous-scene"))
            SceneManager.LoadScene(PlayerPrefs.GetString("previous-scene"));
    }

    public void SetBGMVolume(float linearVulome)
    {
        float dbVolume = LinearToDecibel(linearVulome);
        audioMixer.SetFloat("BGMVolume", dbVolume);
    }

    public void SetBGMSlider()
    {
        float dbVolume;
        audioMixer.GetFloat("BGMVolume", out dbVolume);
        float linearVolume = DecibelToLinear(dbVolume);
        BGMSlider.value = linearVolume;
        initialVolumeBGM = linearVolume;
    }

    public void SetSFXVolume(float linearVulome)
    {
        float dbVolume = LinearToDecibel(linearVulome);
        audioMixer.SetFloat("SFXVolume", dbVolume);
    }

    public void SetSFXSlider()
    {
        float dbVolume;
        audioMixer.GetFloat("SFXVolume", out dbVolume);
        float linearVolume = DecibelToLinear(dbVolume);
        SFXSlider.value = linearVolume;
        initialVolumeSFX = linearVolume;
    }

    private float LinearToDecibel(float linear)
    {
        float dB;
        
        if (linear != 0)
            dB = 20.0f * Mathf.Log10(linear);
        else
            dB = -144.0f;
        
        return dB;
    }

     private float DecibelToLinear(float dB)
     {
         float linear = Mathf.Pow(10.0f, dB/20.0f);
 
         return linear;
     }
}
