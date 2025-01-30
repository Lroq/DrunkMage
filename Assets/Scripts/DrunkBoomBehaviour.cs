using UnityEngine;

public class DrunkBoomBehaviour : MonoBehaviour, ISpellBehaviour
{
    [SerializeField] private GameObject playerr;
    [SerializeField] private GameObject drunkBoomPrefab;

    private GameObject currentDrunkBoom;

    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InvokeSpell()
    {
        if (drunkBoomPrefab == null)
        {
            Debug.LogWarning("Drunk Boom prefab is not assigned.");
            return;
        }

        // Spawn the drunk boom at the player's position
        Vector3 spawnPosition = playerr.transform.position;
        spawnPosition.z -= 1;
        currentDrunkBoom = Instantiate(drunkBoomPrefab, spawnPosition, Quaternion.identity);
        Rigidbody2D drunkBoomRigidbody = currentDrunkBoom.GetComponent<Rigidbody2D>();

        if (drunkBoomRigidbody == null)
        {
            Debug.LogWarning("Drunk Boom prefab does not have a Rigidbody2D component.");
            return;
        }

        // Destroy the drunk boom after a set amount of time
        Destroy(currentDrunkBoom, 0.2f);

        // Slow down the player for a set amount of time
        PlayerBehaviour player = playerr.GetComponent<PlayerBehaviour>();
        player.Speed = 2f;

        // Reset the player speed after a set amount of time
        Invoke("ResetPlayerSpeed", 2f);
    }

    public void CheckIfHitMob()
    {
        if (currentDrunkBoom == null)
        {
            return; // No drunk boom to check
        }

        // Check if the drunk boom hit a mob
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(currentDrunkBoom.transform.position, 1f);
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Mob"))
            {
                Destroy(hitCollider.gameObject);
            }
        }
    }

    public void MoveSpell(GameObject spell)
    {
    }

    private void ResetPlayerSpeed()
    {
        PlayerBehaviour pplayer = playerr.GetComponent<PlayerBehaviour>();
        pplayer.Speed = 5f;
    }

    public void PlaySFX()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }
}
