using UnityEngine;

public class DrunkBoomBehaviour : MonoBehaviour, ISpellBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _drunkBoomPrefab;

    private GameObject _currentDrunkBoom;

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
        if (_drunkBoomPrefab == null)
        {
            Debug.LogWarning("Drunk Boom prefab is not assigned.");
            return;
        }

        // Spawn the drunk boom at the player's position
        Vector3 spawnPosition = _player.transform.position;
        spawnPosition.z -= 1;
        _currentDrunkBoom = Instantiate(_drunkBoomPrefab, spawnPosition, Quaternion.identity);
        Rigidbody2D _drunkBoomRigidbody = _currentDrunkBoom.GetComponent<Rigidbody2D>();

        if (_drunkBoomRigidbody == null)
        {
            Debug.LogWarning("Drunk Boom prefab does not have a Rigidbody2D component.");
            return;
        }

        // Destroy the drunk boom after a set amount of time
        Destroy(_currentDrunkBoom, 0.2f);

        // Slow down the player for a set amount of time
        PlayerBehaviour player = _player.GetComponent<PlayerBehaviour>();
        player.Speed = 2f;
        player.Invoke("ResetSpeed", 1f);
    }

    public void CheckIfHitMob()
    {
        if (_currentDrunkBoom == null)
        {
            return; // No drunk boom to check
        }

        // Check if the drunk boom hit a mob
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(_currentDrunkBoom.transform.position, 1f);
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
}
