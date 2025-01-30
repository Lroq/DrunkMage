using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio; // Required for Audio Mixer

public class SliderManager : MonoBehaviour
{
    public Slider mainVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    public TMP_Text mainVolumeValueText;
    public TMP_Text musicVolumeValueText;
    public TMP_Text sfxVolumeValueText;

    public AudioMixer audioMixer; // Reference to the Audio Mixer

    void Start()
    {
        // Initialize sliders from mixer values
        mainVolumeSlider.value = AudioListener.volume * 100;
        musicVolumeSlider.value = GetVolume("MusicVolume") * 100;
        sfxVolumeSlider.value = GetVolume("SFXVolume") * 100;

        UpdateMainVolumeText(mainVolumeSlider.value);
        UpdateMusicVolumeText(musicVolumeSlider.value);
        UpdateSFXVolumeText(sfxVolumeSlider.value);

        // Add listeners to sliders
        mainVolumeSlider.onValueChanged.AddListener(UpdateMainVolume);
        musicVolumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(UpdateSFXVolume);
    }

    void UpdateMainVolume(float value)
    {
        AudioListener.volume = value / 100f;
        UpdateMainVolumeText(value);
    }

    void UpdateMusicVolume(float value)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(value / 100) * 20); // Convert to decibels
        UpdateMusicVolumeText(value);
    }

    void UpdateSFXVolume(float value)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(value / 100) * 20);
        UpdateSFXVolumeText(value);
    }

    float GetVolume(string paramName)
    {
        float value;
        if (audioMixer.GetFloat(paramName, out value))
            return Mathf.Pow(10, value / 20); // Convert from decibels to linear
        return 1f;
    }

    void UpdateMainVolumeText(float value) => mainVolumeValueText.text = value.ToString("0");
    void UpdateMusicVolumeText(float value) => musicVolumeValueText.text = value.ToString("0");
    void UpdateSFXVolumeText(float value) => sfxVolumeValueText.text = value.ToString("0");

    void OnDestroy()
    {
        mainVolumeSlider.onValueChanged.RemoveListener(UpdateMainVolume);
        musicVolumeSlider.onValueChanged.RemoveListener(UpdateMusicVolume);
        sfxVolumeSlider.onValueChanged.RemoveListener(UpdateSFXVolume);
    }
}
