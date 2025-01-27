using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderManager : MonoBehaviour
{

    public Slider mainVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    public TMP_Text mainVolumeValueText;
    public TMP_Text musicVolumeValueText;
    public TMP_Text sfxVolumeValueText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateMainVolumeText(mainVolumeSlider.value);
        UpdateMusicVolumeText(musicVolumeSlider.value);
        UpdateSFXVolumeText(sfxVolumeSlider.value);

        mainVolumeSlider.onValueChanged.AddListener(UpdateMainVolumeText);
        musicVolumeSlider.onValueChanged.AddListener(UpdateMusicVolumeText);
        sfxVolumeSlider.onValueChanged.AddListener(UpdateSFXVolumeText);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateMainVolumeText(float value)
    {
        mainVolumeValueText.text = value.ToString("0");
    }

    void UpdateMusicVolumeText(float value)
    {
        musicVolumeValueText.text = value.ToString("0");
    }

    void UpdateSFXVolumeText(float value)
    {
        sfxVolumeValueText.text = value.ToString("0");
    }

    void OnDestroy()
    {
        mainVolumeSlider.onValueChanged.RemoveListener(UpdateMainVolumeText);
        musicVolumeSlider.onValueChanged.RemoveListener(UpdateMusicVolumeText);
        sfxVolumeSlider.onValueChanged.RemoveListener(UpdateSFXVolumeText);
    }
}
