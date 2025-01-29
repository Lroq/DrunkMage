using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // For quitting to main menu or desktop

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject dimBackground;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button quitToMainMenuButton;
    private bool isPaused = false;

    public AudioClip mainMenuMusicMusic;
    private MusicManager musicManager;

    void Start()
    {
        // Add listeners to the buttons
        resumeButton.onClick.AddListener(ResumeGame);
        quitToMainMenuButton.onClick.AddListener(QuitToMainMenu);
    }

    void Update()
    {
        // Check for Pause/Unpause input (Escape key)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            ActivatePauseMenu();
        }
        else
        {
            DeactivatePauseMenu();
        }
    }

    private void ActivatePauseMenu()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        pauseMenu.SetActive(true);
        dimBackground.SetActive(true);
    }

    private void DeactivatePauseMenu()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        pauseMenu.SetActive(false);
        dimBackground.SetActive(false);
    }

    // Called when the "Resume" button is pressed
    public void ResumeGame()
    {
        DeactivatePauseMenu();
    }

    // Called when the "Quit to Main Menu" button is pressed
    public void QuitToMainMenu()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        SceneManager.LoadScene("MainMenu");
        MusicManager musicManager = FindFirstObjectByType<MusicManager>();
        musicManager.StopMusic();
        musicManager.PlayMusic(mainMenuMusicMusic);
    }
}
