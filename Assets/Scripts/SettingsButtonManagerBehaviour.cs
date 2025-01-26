using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsButtonManagerBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject BackButton;
    [SerializeField] private GameObject AudioButton;
    [SerializeField] private GameObject VideoButton;
    [SerializeField] private GameObject ControlsButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BackButton.GetComponent<Button>().onClick.AddListener(OnBackButtonClicked);
        AudioButton.GetComponent<Button>().onClick.AddListener(OnAudioButtonClicked);
        VideoButton.GetComponent<Button>().onClick.AddListener(OnVideoButtonClicked);
        ControlsButton.GetComponent<Button>().onClick.AddListener(OnControlsButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnEscapeButtonClicked();
        }
    }

    public void OnBackButtonClicked()
    {
        Debug.Log("Back Button Clicked");
        SceneManager.LoadScene("MainMenu");
    }

    public void OnAudioButtonClicked()
    {
        Debug.Log("Audio Button Clicked");
        SceneManager.LoadScene("AudioMenu");
    }

    public void OnVideoButtonClicked()
    {
        Debug.Log("Video Button Clicked");
        SceneManager.LoadScene("VideoMenu");
    }

    public void OnControlsButtonClicked()
    {
        Debug.Log("Controls Button Clicked");
        SceneManager.LoadScene("ControlsMenu");
    }

    public void OnEscapeButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
