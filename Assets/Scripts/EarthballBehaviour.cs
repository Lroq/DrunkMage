using UnityEngine;

public class EarthballBehaviour : MonoBehaviour, ISpellBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject earthballPrefab;
    [SerializeField] private float orbitRadius = 2f; // Distance from the player
    [SerializeField] private float orbitSpeed = 50f; // Speed of the orbit in degrees per second

    private GameObject currentEarthball;
    private float orbitAngle; // Current angle of the orbit
    private AudioSource audioSource;

    // Update is called once per frame
    void Update()
    {
        // Make the earthball orbit around the player
        MoveSpell(currentEarthball);

        // Optionally check for mob collisions while orbiting
        CheckIfHitMob();
    }

    // Spawn the earthball at the start of the orbit
    public void InvokeSpell()
    {
        if (earthballPrefab == null)
        {
            Debug.LogWarning("Earthball prefab is not assigned.");
            return;
        }

        if (player == null)
        {
            Debug.LogWarning("Player object is not assigned.");
            return;
        }

        // Calculate the initial spawn position (at the orbit radius on the right side)
        Vector3 spawnPosition = player.transform.position;
        spawnPosition.x += orbitRadius;

        // Instantiate the earthball
        currentEarthball = Instantiate(earthballPrefab, spawnPosition, Quaternion.identity);

        DestroyEarthball();
    }

    // Make the earthball orbit around the player
    public void MoveSpell(GameObject spell)
    {
        if (spell == null || player == null)
        {
            return;
        }

        // Increment the orbit angle based on the speed and time
        orbitAngle += orbitSpeed * Time.deltaTime;
        orbitAngle %= 360; // Keep the angle within 0-360 degrees

        // Convert the angle to radians and calculate the new position
        float radians = orbitAngle * Mathf.Deg2Rad;
        Vector3 playerPosition = player.transform.position;
        float x = playerPosition.x + orbitRadius * Mathf.Cos(radians);
        float y = playerPosition.y + orbitRadius * Mathf.Sin(radians);

        // Update the earthball's position
        spell.transform.position = new Vector3(x, y, spell.transform.position.z);
    }

    // Check if the earthball hit a mob
    public void CheckIfHitMob()
    {
        if (currentEarthball == null)
        {
            return;
        }

        // Detect nearby mobs using OverlapCircle
        Collider2D[] colliders = Physics2D.OverlapCircleAll(currentEarthball.transform.position, 0.5f);
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
        if (currentEarthball == null)
        {
            return;
        }

        Destroy(currentEarthball, 10.0f); // Example lifetime of 10 seconds
    }

    public void PlaySFX()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }
}
