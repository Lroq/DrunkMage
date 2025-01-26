using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ResolutionDropdown : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown; // Reference to the Dropdown UI component
    public Toggle fullscreenToggle; // Reference to the Toggle UI component

    // List of supported resolutions
    private Resolution[] resolutions = {
        new Resolution { width = 1920, height = 1080 },
        new Resolution { width = 1600, height = 900 },
        new Resolution { width = 1366, height = 768 },
        new Resolution { width = 1280, height = 720 }
    };

    void Start()
    {
        // Populate the dropdown with resolution options
        resolutionDropdown.ClearOptions();

        var options = new System.Collections.Generic.List<string>();
        foreach (var res in resolutions)
        {
            options.Add($"{res.width}x{res.height}");
        }
        resolutionDropdown.AddOptions(options);

        // Set the initial value based on the current screen resolution
        SetInitialDropdownValue();
        fullscreenToggle.isOn = Screen.fullScreen;

        // Add a listener to handle resolution changes
        resolutionDropdown.onValueChanged.AddListener(ChangeResolution);
        fullscreenToggle.onValueChanged.AddListener(ToggleFullscreen);
    }

    void Update()
    {
        // Check for Escape key press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnEscapeButtonClicked();
        }
    }

    void SetInitialDropdownValue()
    {
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (Screen.width == resolutions[i].width && Screen.height == resolutions[i].height)
            {
                resolutionDropdown.value = i;
                return;
            }
        }
    }

    void ChangeResolution(int index)
    {
        // Get the selected resolution
        var selectedResolution = resolutions[index];

        // Change the screen resolution
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
    }

    void ToggleFullscreen(bool isFullscreen)
    {
        // Change the fullscreen mode
        Screen.fullScreen = isFullscreen;
    }

    void OnDestroy()
    {
        // Remove the listener to avoid memory leaks
        resolutionDropdown.onValueChanged.RemoveListener(ChangeResolution);
    }

    public void OnEscapeButtonClicked()
    {
        SceneManager.LoadScene("SettingsMenu");
    }
}
