using UnityEngine;

public class TempestBehaviour : MonoBehaviour, ISpellBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float destroyTime = 10.0f;
    [SerializeField] private GameObject tempestPrefab;
    [SerializeField] private Camera mainCamera;

    private GameObject currentTempest;
    private Rigidbody2D tempestRigidbody;
    private Vector2 currentDirection;
    private AudioSource audioSource;

    void Start()
    {
    }

    void Update()
    {
        MoveSpell(currentTempest);
        CheckIfHitMob();
    }

    public void InvokeSpell()
    {
        if (tempestPrefab == null)
        {
            Debug.LogWarning("Tempest prefab is not assigned.");
            return;
        }

        // Spawn the tempest at the player's position
        currentTempest = Instantiate(tempestPrefab, transform.position, Quaternion.identity);
        tempestRigidbody = currentTempest.GetComponent<Rigidbody2D>();

        if (tempestRigidbody == null)
        {
            Debug.LogWarning("Tempest prefab does not have a Rigidbody2D component.");
            return;
        }

        // Initialize a random direction for the tempest
        currentDirection = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;

        Destroy(currentTempest, destroyTime);
    }

    // Move the tempest from the player position into a random direction within the screen bounds and make it bounce off the screen edges
    public void MoveSpell(GameObject spell)
    {
        if (spell != null)
        {
            Vector2 position = spell.transform.position;
            Vector2 screenPosition = mainCamera.WorldToScreenPoint(position);

            if (screenPosition.x < 0 || screenPosition.x > Screen.width)
            {
                currentDirection.x *= -1;
            }

            if (screenPosition.y < 0 || screenPosition.y > Screen.height)
            {
                currentDirection.y *= -1;
            }

            tempestRigidbody.linearVelocity = currentDirection * speed;
        }
    }

    public void CheckIfHitMob()
    {
        if (currentTempest == null)
        {
            return; // No tempest to check
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(currentTempest.transform.position, 0.5f);
        foreach (Collider2D collider in colliders)
        {
            OnEnterTrigger(collider);
        }
    }

    private void OnEnterTrigger(Collider2D collider)
    {
        if (collider.CompareTag("Mob"))
        {
            // Handle mob hit
            Debug.Log("Tempest hit mob: " + collider.name);
            Destroy(collider.gameObject);
        }
    }

    public void PlaySFX()
    {
        // Play the tempest sound effect
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
