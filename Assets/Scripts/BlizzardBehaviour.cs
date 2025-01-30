using UnityEngine;

public class BlizzardBehaviour : MonoBehaviour, ISpellBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject blizzardPrefab;

    private float blizzardLifetime = 10.0f;
    private GameObject currentBlizzard;
    private Rigidbody2D blizzardRigidbody;
    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveSpell(currentBlizzard);
    }

    //Spawn the blizzard at the player's position
    public void InvokeSpell()
    {
        if (blizzardPrefab == null)
        {
            Debug.LogWarning("Blizzard prefab is not assigned.");
            return;
        }

        // Spawn the blizzard at the player's position
        Vector3 spawnPosition = player.transform.position;
        spawnPosition.z -= 1;
        currentBlizzard = Instantiate(blizzardPrefab, spawnPosition, Quaternion.identity);
        blizzardRigidbody = currentBlizzard.GetComponent<Rigidbody2D>();

        if (blizzardRigidbody == null)
        {
            Debug.LogWarning("Blizzard prefab does not have a Rigidbody2D component.");
            return;
        }

        DestroyBlizzard();
    }

    //Make the blizzard follow the player
    public void MoveSpell(GameObject spell)
    {
        if (spell != null)
        {
            Vector2 playerPosition = player.transform.position;
            spell.transform.position = playerPosition;
        }
    }

    //Check if the blizzard hit a mob
    public void CheckIfHitMob()
    {
        if (currentBlizzard == null)
        {
            return; // No blizzard to check
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(currentBlizzard.transform.position, 1.0f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Mob"))
            {
                Destroy(collider.gameObject);
            }
        }
    }

    //Destroy the blizzard after a certain amount of time
    public void DestroyBlizzard()
    {
        if (currentBlizzard == null)
        {
            return; // No blizzard to destroy
        }

        Destroy(currentBlizzard, blizzardLifetime);
    }

    //Play the blizzard sound effect
    public void PlaySFX()
    {
        // Play the blizzard sound effect
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
