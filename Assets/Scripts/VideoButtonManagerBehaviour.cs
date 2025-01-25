using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VideoButtonManagerBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject BackButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BackButton.GetComponent<Button>().onClick.AddListener(OnBackButtonClicked);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBackButtonClicked()
    {
        SceneManager.LoadScene("SettingsMenu");
    }
}
