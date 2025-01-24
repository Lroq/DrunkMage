using UnityEngine;

public class BlizzardBehaviour : MonoBehaviour, ISpellBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _blizzardPrefab;

    private float _blizzardLifetime = 10.0f;
    private GameObject _currentBlizzard;
    private Rigidbody2D _blizzardRigidbody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveSpell(_currentBlizzard);
    }

    //Spawn the blizzard at the player's position
    public void InvokeSpell()
    {
        if (_blizzardPrefab == null)
        {
            Debug.LogWarning("Blizzard prefab is not assigned.");
            return;
        }

        // Spawn the blizzard at the player's position
        Vector3 spawnPosition = _player.transform.position;
        spawnPosition.z -= 1;
        _currentBlizzard = Instantiate(_blizzardPrefab, spawnPosition, Quaternion.identity);
        _blizzardRigidbody = _currentBlizzard.GetComponent<Rigidbody2D>();

        if (_blizzardRigidbody == null)
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
            Vector2 playerPosition = _player.transform.position;
            spell.transform.position = playerPosition;
        }
    }

    //Check if the blizzard hit a mob
    public void CheckIfHitMob()
    {
        if (_currentBlizzard == null)
        {
            return; // No blizzard to check
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_currentBlizzard.transform.position, 1.0f);
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
        if (_currentBlizzard == null)
        {
            return; // No blizzard to destroy
        }

        Destroy(_currentBlizzard, _blizzardLifetime);
    }
}
