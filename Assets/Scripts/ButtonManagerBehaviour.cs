using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ButtonManagerBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject QuitButton;
    [SerializeField] private GameObject PlayButton;
    [SerializeField] private GameObject SettingsButton;
    public AudioClip playMusic;
    public AudioClip mainMenuMusicMusic;

    private MusicManager musicManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        QuitButton.GetComponent<Button>().onClick.AddListener(OnQuitButtonClicked);
        PlayButton.GetComponent<Button>().onClick.AddListener(OnPlayButtonClicked);
        SettingsButton.GetComponent<Button>().onClick.AddListener(OnSettingsButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayButtonClicked()
    {
        musicManager = FindFirstObjectByType<MusicManager>();
        musicManager.StopMusic();
        musicManager.PlayMusic(playMusic);
        SceneManager.LoadScene("TestingSpell");

        Debug.Log("Play Button Clicked");
    }

    public void OnSettingsButtonClicked()
    {
        SceneManager.LoadScene("SettingsMenu");
        Debug.Log("Settings Button Clicked");
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
        
        Debug.Log("Quit Button Clicked");
    }
}
