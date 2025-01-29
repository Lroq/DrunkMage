using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioClip mainMenuMusic;
    private AudioSource audioSource;

    // Singleton to ensure only one MusicManager instance
    private static MusicManager instance;

    void Awake()
    {
        // Ensure only one instance of MusicManager persists across scenes
        if (instance != null && instance != this)
        {
            Destroy(gameObject);  // Destroy duplicate
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Keep across scenes
        }
    }

    void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();

        // Play the music for the main menu (or current scene)
        PlayMusic(mainMenuMusic);
    }

    public void StopMusic()
    {
        // Stop the current music
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        // Set the new music clip and play it
        audioSource.clip = clip;
        audioSource.Play();
    }
}
