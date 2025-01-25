using UnityEngine;

public class EarthballBehaviour : MonoBehaviour, ISpellBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _earthballPrefab;
    [SerializeField] private float orbitRadius = 2f; // Distance from the player
    [SerializeField] private float orbitSpeed = 50f; // Speed of the orbit in degrees per second

    private GameObject _currentEarthball;
    private float _orbitAngle; // Current angle of the orbit

    // Update is called once per frame
    void Update()
    {
        // Make the earthball orbit around the player
        MoveSpell(_currentEarthball);

        // Optionally check for mob collisions while orbiting
        CheckIfHitMob();
    }

    // Spawn the earthball at the start of the orbit
    public void InvokeSpell()
    {
        if (_earthballPrefab == null)
        {
            Debug.LogWarning("Earthball prefab is not assigned.");
            return;
        }

        if (_player == null)
        {
            Debug.LogWarning("Player object is not assigned.");
            return;
        }

        // Calculate the initial spawn position (at the orbit radius on the right side)
        Vector3 spawnPosition = _player.transform.position;
        spawnPosition.x += orbitRadius;

        // Instantiate the earthball
        _currentEarthball = Instantiate(_earthballPrefab, spawnPosition, Quaternion.identity);

        DestroyEarthball();
    }

    // Make the earthball orbit around the player
    public void MoveSpell(GameObject spell)
    {
        if (spell == null || _player == null)
        {
            return;
        }

        // Increment the orbit angle based on the speed and time
        _orbitAngle += orbitSpeed * Time.deltaTime;
        _orbitAngle %= 360; // Keep the angle within 0-360 degrees

        // Convert the angle to radians and calculate the new position
        float radians = _orbitAngle * Mathf.Deg2Rad;
        Vector3 playerPosition = _player.transform.position;
        float x = playerPosition.x + orbitRadius * Mathf.Cos(radians);
        float y = playerPosition.y + orbitRadius * Mathf.Sin(radians);

        // Update the earthball's position
        spell.transform.position = new Vector3(x, y, spell.transform.position.z);
    }

    // Check if the earthball hit a mob
    public void CheckIfHitMob()
    {
        if (_currentEarthball == null)
        {
            return;
        }

        // Detect nearby mobs using OverlapCircle
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_currentEarthball.transform.position, 0.5f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag("Mob"))
            {
                // Destroy the mob and optionally do something with the earthball
                Destroy(collider.gameObject);
            }
        }
    }

    // Destroy the earthball after a certain amount of time
    public void DestroyEarthball()
    {
        if (_currentEarthball == null)
        {
            return;
        }

        Destroy(_currentEarthball, 10.0f); // Example lifetime of 10 seconds
    }
}
