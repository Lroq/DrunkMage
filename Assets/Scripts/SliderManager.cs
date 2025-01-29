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

    private AudioSource musicAudioSource;  // Music audio source
    private AudioSource sfxAudioSource;    // SFX audio source

    void Start()
    {
        // Get references to the audio sources (assumed to be attached to the same GameObject)
        musicAudioSource = FindFirstObjectByType<MusicManager>().GetComponent<AudioSource>();  // Assuming MusicManager has the AudioSource
        //sfxAudioSource = FindFirstObjectByType<SFXManager>().GetComponent<AudioSource>();    // Assuming SFXManager has the AudioSource

        // Initialize slider values and text displays
        mainVolumeSlider.value = AudioListener.volume * 100;
        musicVolumeSlider.value = musicAudioSource.volume * 100;
        //sfxVolumeSlider.value = sfxAudioSource.volume;

        UpdateMainVolumeText(mainVolumeSlider.value);
        UpdateMusicVolumeText(musicVolumeSlider.value);
        UpdateSFXVolumeText(sfxVolumeSlider.value);

        // Add listeners to sliders
        mainVolumeSlider.onValueChanged.AddListener(UpdateMainVolume);
        musicVolumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
        //sfxVolumeSlider.onValueChanged.AddListener(UpdateSFXVolume);
    }

    // Update main volume based on slider value
    void UpdateMainVolume(float value)
    {
        AudioListener.volume = value / 100f;  // Adjust application-wide volume
        UpdateMainVolumeText(value);
    }

    // Update music volume based on slider value
    void UpdateMusicVolume(float value)
    {
        if (musicAudioSource != null)
        {
            musicAudioSource.volume = value / 100f;  // Adjust music volume
        }
        UpdateMusicVolumeText(value);
    }

    // Update SFX volume based on slider value
    void UpdateSFXVolume(float value)
    {
        if (sfxAudioSource != null)
        {
            sfxAudioSource.volume = value;  // Adjust SFX volume
        }
        UpdateSFXVolumeText(value);
    }

    // Update volume text for main volume
    void UpdateMainVolumeText(float value)
    {
        mainVolumeValueText.text = value.ToString("0");
    }

    // Update volume text for music volume
    void UpdateMusicVolumeText(float value)
    {
        musicVolumeValueText.text = value.ToString("0");
    }

    // Update volume text for SFX volume
    void UpdateSFXVolumeText(float value)
    {
        sfxVolumeValueText.text = value.ToString("0");
    }

    void OnDestroy()
    {
        // Remove listeners when the object is destroyed
        mainVolumeSlider.onValueChanged.RemoveListener(UpdateMainVolume);
        musicVolumeSlider.onValueChanged.RemoveListener(UpdateMusicVolume);
        sfxVolumeSlider.onValueChanged.RemoveListener(UpdateSFXVolume);
    }
}
