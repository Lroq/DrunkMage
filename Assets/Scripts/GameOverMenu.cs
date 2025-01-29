using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private GameObject gameOverMenu; // Reference to the Game Over menu
    [SerializeField] private GameObject gameOverBackground; // Gradient black panel
    [SerializeField] private Image backgroundImage; // Image component of the background
    [SerializeField] private GameObject replayButton; // Reference to the Replay button
    [SerializeField] private GameObject quitButton; // Reference to the Quit button

    public AudioClip mainMenuMusic;
    public AudioClip gameplayMusic;

    void Awake()
    {
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
        SceneManager.LoadScene("MainMenu");
        AudioListener.pause = false;

        MusicManager musicManager = FindFirstObjectByType<MusicManager>();
        if (musicManager != null)
        {
            musicManager.PlayMusic(mainMenuMusic);
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
