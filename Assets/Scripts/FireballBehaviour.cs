using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class FireballBehaviour : MonoBehaviour, ISpellBehaviour
{
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private GameObject Player;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float speed = 5f;

    [SerializeField] private List<GameObject> fireballs = new List<GameObject>();

    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (fireballPrefab == null)
        {
            Debug.LogWarning("Fireball prefab is not assigned.");
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void InvokeSpell()
    {
        if (fireballPrefab == null)
        {
            Debug.LogWarning("Fireball prefab is not assigned.");
            return;
        }

        // Spawn the fireball at the player's position
        GameObject fireball = Instantiate(fireballPrefab, Player.transform.position, Quaternion.identity);
        fireballs.Add(fireball);

        // Move the fireball (delegated to MoveSpell)
        MoveSpell(fireball);

        // Destroy the fireball after 2 seconds
        Destroy(fireball, 2f);

        RotateCurrentFireballTowardsMouse();
    }

    // Move the fireball towards the mouse position
    public void MoveSpell(GameObject spell)
    {
        if (spell != null)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector2 worldMousePosition = mainCamera.ScreenToWorldPoint(mousePosition);

            Vector2 direction = (worldMousePosition - (Vector2)spell.transform.position).normalized;
            spell.GetComponent<Rigidbody2D>().linearVelocity = direction * speed;
        }
    }

    // Check if the fireball hit the mob tag
    public void CheckIfHitMob()
    {
        foreach (GameObject fireball in fireballs)
        {
            if (fireball != null)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(fireball.transform.position, 0.1f);
                foreach (Collider2D collider in colliders)
                {
                    //OnEnterTrigger(collider);
                    if (collider.CompareTag("Mob"))
                    {
                        Debug.Log("Fireball hit mob");
                        Destroy(collider.gameObject);
                        Destroy(fireball);
                    }
                }
            }
        }
    }

    // Make the fireball watch towards the mouse position at the time of shooting
    public void RotateCurrentFireballTowardsMouse()
    {
        if (fireballs.Count > 0)
        {
            GameObject currentFireball = fireballs[fireballs.Count - 1];
            if (currentFireball != null)
            {
                Vector2 mousePosition = Mouse.current.position.ReadValue();
                Vector2 worldMousePosition = mainCamera.ScreenToWorldPoint(mousePosition);

                Vector2 direction = (worldMousePosition - (Vector2)currentFireball.transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                currentFireball.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
    }

    public void PlaySFX()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }
}