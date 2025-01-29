using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int totalLives = 3; // The total number of lives the player has
    [SerializeField] private int currentLives; // The current number of lives the player has
    public TextMeshProUGUI livesText; // UI Text to display current lives
    public GameOverMenu gameOverMenu; // The Game Over UI menu
    public GameObject player; // Reference to the player (optional if you want to disable movement or other features on game over)

    void Start()
    {
        currentLives = totalLives;
        UpdateLivesText();
        gameOverMenu.HideGameOverMenu(); // Hide Game Over menu initially
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
        UpdateLivesText();

        if (currentLives <= 0)
        {
            GameOver();
        }
    }

    // Update the lives UI text
    void UpdateLivesText()
    {
        livesText.text = "Lives: " + currentLives.ToString();
    }

    // Trigger the Game Over logic
    void GameOver()
    {
        gameOverMenu.ShowGameOverMenu(); // Show the Game Over menu
        // Optionally, disable player movement or any other game mechanics here
        Time.timeScale = 0f; // Stop the game (optional, if you want to pause the game when Game Over)
        AudioListener.pause = true; // Pause the audio listener (optional, if you want to pause the audio when Game Over)
    }

    public int GetCurrentLives()
    {
        return currentLives;
    }
}
