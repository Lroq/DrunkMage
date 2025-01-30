using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Audio;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject gameOverBackground;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private GameObject replayButton;
    [SerializeField] private GameObject quitButton;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioClip buttonClickSFX;

    public AudioClip mainMenuMusic;
    public AudioClip gameplayMusic;
    private AudioSource audioSource;

    void Awake()
    {
        // Ensure that AudioSource is present
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found. Please add an AudioSource component to this GameObject.");
        }

        // Load the button click SFX once during initialization
        buttonClickSFX = Resources.Load<AudioClip>("ButtonClickSFX");
        if (buttonClickSFX == null)
        {
            Debug.LogError("ButtonClickSFX not found in Resources folder.");
        }

        // Set up the button listeners
        replayButton.GetComponent<Button>().onClick.AddListener(OnReplayButtonClicked);
        quitButton.GetComponent<Button>().onClick.AddListener(OnQuitButtonClicked);

        gameOverMenu.SetActive(false); // Hide Game Over menu initially
        gameOverBackground.SetActive(false); // Hide background initially
    }

    public void ShowGameOverMenu()
    {
        gameOverBackground.SetActive(true); // Activate background before fading
        gameOverMenu.SetActive(true); // Activate Game Over menu
        StartCoroutine(FadeInBackground()); // Now it can run
    }

    public void HideGameOverMenu()
    {
        gameOverMenu.SetActive(false);
        gameOverBackground.SetActive(false);
    }

    IEnumerator FadeInBackground()
    {
        Color color = backgroundImage.color;
        color.a = 0; // Start fully transparent
        backgroundImage.color = color;

        float duration = 1.5f;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            color.a = Mathf.Lerp(0, 1, elapsedTime / duration);
            backgroundImage.color = color;
            yield return null;
        }
    }

    public void ReplayGame()
    {
        Time.timeScale = 1f;
        PlayButtonClickSFX();
        audioMixer.SetFloat("MusicVolume", 1f); // Reset the music volume
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        AudioListener.pause = false;

        MusicManager musicManager = FindFirstObjectByType<MusicManager>();
        if (musicManager != null)
        {
            musicManager.PlayMusic(gameplayMusic);
        }
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        PlayButtonClickSFX();
        audioMixer.SetFloat("MusicVolume", 1f); // Reset the music volume
        SceneManager.LoadScene("MainMenu");
        AudioListener.pause = false;

        MusicManager musicManager = FindFirstObjectByType<MusicManager>();
        if (musicManager != null)
        {
            musicManager.PlayMusic(mainMenuMusic);
        }
    }

    private void PlayButtonClickSFX()
    {
        if (audioSource != null && buttonClickSFX != null)
        {
            audioSource.PlayOneShot(buttonClickSFX);
        }
        else if (audioSource == null)
        {
            Debug.LogWarning("AudioSource is not assigned.");
        }
        else
        {
            Debug.LogWarning("ButtonClickSFX is not assigned.");
        }
    }

    public void OnReplayButtonClicked()
    {
        ReplayGame();
    }

    public void OnQuitButtonClicked()
    {
        QuitGame();
    }
}
