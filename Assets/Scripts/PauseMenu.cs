using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseCanvas;
    public bool paused;
    // Start is called before the first frame update
    public AudioMixerSnapshot pausedSnapshot;
    public AudioMixerSnapshot unpausedSnapshot;
    private float initailBGMVolume;

    [Header("Modern UI Settings")]
    public AudioSource buttonRollover;
    public AudioSource buttonClick;
    public float animationSpeed = 15f;
    public float hoverScale = 1.1f;
    public float clickScale = 0.95f;

    void Start()
    {
        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Confined;
        paused = false;
        Time.timeScale = 1;

        // Ensure listeners are added (Original Logic)
        Button restartBtn = pauseCanvas.transform.Find("RestartButton").gameObject.GetComponent<Button>();
        Button menuBtn = pauseCanvas.transform.Find("MenuButton").gameObject.GetComponent<Button>();
        Button optionsBtn = pauseCanvas.transform.Find("OptionsButton").gameObject.GetComponent<Button>();
        Button resumeBtn = pauseCanvas.transform.Find("ResumeButton").gameObject.GetComponent<Button>();

        restartBtn.onClick.AddListener(Restart);
        menuBtn.onClick.AddListener(MainMenu);
        optionsBtn.onClick.AddListener(Options);
        resumeBtn.onClick.AddListener(Resume);

        // Setup Modern UI
        SetupModernButton(restartBtn);
        SetupModernButton(menuBtn);
        SetupModernButton(optionsBtn);
        SetupModernButton(resumeBtn);

        unpausedSnapshot.TransitionTo(0.01f);
        SceneManager.activeSceneChanged += ChangeActiveScene;
    }

    void SetupModernButton(Button btn)
    {
        if (btn == null) return;
        ModernMenuButton modernBtn = btn.gameObject.GetComponent<ModernMenuButton>();
        if (modernBtn == null) modernBtn = btn.gameObject.AddComponent<ModernMenuButton>();
        modernBtn.Initialize(buttonRollover, buttonClick, hoverScale, clickScale, animationSpeed);
    }

    void ChangeActiveScene(Scene current, Scene next)
    {
        Time.timeScale = 1;
        unpausedSnapshot.TransitionTo(0.01f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
                Resume();
            else
                Pause();
        }        
    }

    public void Pause()
    {
        // Cursor.visible = true;
        // Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        pauseCanvas.SetActive(true);
        paused = true;
        pausedSnapshot.TransitionTo(0.01f);
        
        // Trigger Entrance Animation
        StartCoroutine(PlayEntranceAnimation());
    }

    IEnumerator PlayEntranceAnimation()
    {
        Button[] buttons = pauseCanvas.GetComponentsInChildren<Button>();
        foreach (var btn in buttons)
        {
            btn.transform.localScale = Vector3.zero;
        }

        foreach (var btn in buttons)
        {
            StartCoroutine(AnimateScale(btn.transform, Vector3.one));
            yield return new WaitForSecondsRealtime(0.1f); // Use Realtime since TimeScale is 0
        }
    }

    IEnumerator AnimateScale(Transform target, Vector3 endScale)
    {
        Vector3 startScale = target.localScale;
        float t = 0;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * animationSpeed; // Unscaled for pause menu
            target.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }
        target.localScale = endScale;
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseCanvas.SetActive(false);
        paused = false;
        unpausedSnapshot.TransitionTo(0.01f);
        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Confined;
    }

    public void Restart()
    {
        PlayClickSound();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        PlayClickSound();
        SceneManager.LoadScene("MainMenu");
    }

    public void Options()
    {
        PlayClickSound();
        PlayerPrefs.SetString("previous-scene", SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("Options");
    }

    void PlayClickSound()
    {
        if (buttonClick != null) buttonClick.Play();
    }
}
