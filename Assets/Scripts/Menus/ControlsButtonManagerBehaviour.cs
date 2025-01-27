using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControlsButtonManagerBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject BackButton;
    [SerializeField] private GameObject CheckControlsButton;
    [SerializeField] private GameObject ChangeControlsButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BackButton.GetComponent<Button>().onClick.AddListener(OnBackButtonClicked);
        CheckControlsButton.GetComponent<Button>().onClick.AddListener(OnCheckControlsButtonClicked);
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
        SceneManager.LoadScene("SettingsMenu");
        
    }

    public void OnCheckControlsButtonClicked()
    {
        Debug.Log("Check Controls Button Clicked");
        SceneManager.LoadScene("ControlsDisplayMenu");
    }

    public void OnEscapeButtonClicked()
    {
        SceneManager.LoadScene("SettingsMenu");
    }
}
