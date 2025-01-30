using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int currentLives; // The current number of lives the player has
    [SerializeField] private AudioMixer audioMixer; // Reference to the Audio Mixer
    [SerializeField] private AudioClip hurtSFX; // Sound effect when player is hit

    public int totalLives = 3; // The total number of lives the player has
    public GameObject[] hearts; // Array of heart images for each life
    public GameOverMenu gameOverMenu; // The Game Over UI menu
    public GameObject player; // Reference to the player (optional if you want to disable movement or other features on game over)

    private AudioSource audioSource;

    void Start()
    {
        currentLives = totalLives;
        UpdateLivesDisplay();
        gameOverMenu.HideGameOverMenu(); // Hide Game Over menu initially

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource component not found.");
        }

        audioMixer = Resources.Load<AudioMixer>("AudioMixer");
        if (audioMixer == null)
        {
            Debug.LogError("AudioMixer not found. Make sure it's in Resources folder.");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Mob")) // Ensure your mobs are tagged as "Mob"
        {
            TakeDamage();
        }
    }

    // Call this function when the player is hit by a mob
    public void TakeDamage()
    {
        currentLives--;
        UpdateLivesDisplay();

        if (audioSource != null && hurtSFX != null)
        {
            audioSource.PlayOneShot(hurtSFX);  // Play hurt sound effect
        }
        else
        {
            Debug.LogWarning("Hurt SFX or AudioSource is not assigned.");
        }

        if (currentLives <= 0)
        {
            GameOver();
        }
    }

    // Update the UI to reflect the player's remaining lives
    void UpdateLivesDisplay()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentLives)
            {
                hearts[i].SetActive(true); // Enable heart if player has that life
            }
            else
            {
                hearts[i].SetActive(false); // Disable heart if player doesn't have that life
            }
        }
    }

    // Trigger the Game Over logic
    void GameOver()
    {
        audioMixer.SetFloat("MusicVolume", -80f); // Lower the music volume
        gameOverMenu.ShowGameOverMenu(); // Show the Game Over menu
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play(); // Stop the audio source
        } 
        else
        {
            Debug.LogWarning("Audio clip not found.");
        }

        Invoke("PauseGame", 0.5f); // Pause the game after a delay
    }

    void PauseGame()
    {
        Time.timeScale = 0f; // Stop the game
        AudioListener.pause = true; // Pause the audio listener
    }

    public int GetCurrentLives()
    {
        return currentLives;
    }
}
