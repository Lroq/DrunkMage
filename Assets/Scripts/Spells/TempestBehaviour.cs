using UnityEngine;

public class TempestBehaviour : MonoBehaviour, ISpellBehaviour
{
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private float _destroyTime = 10.0f;
    [SerializeField] private GameObject _tempestPrefab;
    [SerializeField] private Camera _mainCamera;

    private GameObject _currentTempest;
    private Rigidbody2D _tempestRigidbody;
    private Vector2 _currentDirection;

    void Start()
    {
    }

    void Update()
    {
        MoveSpell(_currentTempest);
        CheckIfHitMob();
    }

    public void InvokeSpell()
    {
        if (_tempestPrefab == null)
        {
            Debug.LogWarning("Tempest prefab is not assigned.");
            return;
        }

        // Spawn the tempest at the player's position
        _currentTempest = Instantiate(_tempestPrefab, transform.position, Quaternion.identity);
        _tempestRigidbody = _currentTempest.GetComponent<Rigidbody2D>();

        if (_tempestRigidbody == null)
        {
            Debug.LogWarning("Tempest prefab does not have a Rigidbody2D component.");
            return;
        }

        // Initialize a random direction for the tempest
        _currentDirection = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;

        Destroy(_currentTempest, _destroyTime);
    }

    // Move the tempest from the player position into a random direction within the screen bounds and make it bounce off the screen edges
    public void MoveSpell(GameObject spell)
    {
        if (spell != null)
        {
            Vector2 position = spell.transform.position;
            Vector2 screenPosition = _mainCamera.WorldToScreenPoint(position);

            if (screenPosition.x < 0 || screenPosition.x > Screen.width)
            {
                _currentDirection.x *= -1;
            }

            if (screenPosition.y < 0 || screenPosition.y > Screen.height)
            {
                _currentDirection.y *= -1;
            }

            _tempestRigidbody.linearVelocity = _currentDirection * _speed;
        }
    }

    public void CheckIfHitMob()
    {
        if (_currentTempest == null)
        {
            return; // No tempest to check
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_currentTempest.transform.position, 0.5f);
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
}
