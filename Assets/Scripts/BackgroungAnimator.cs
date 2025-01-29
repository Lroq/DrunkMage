using UnityEngine;

public class BackgroundAnimator : MonoBehaviour
{
    public float speed = 0.5f; // Speed of background movement
    public float movementRange = 2f; // Horizontal range of movement

    private Vector3 startPosition;

    void Start()
    {
        // Store the initial position
        startPosition = transform.position;
    }

    void Update()
    {
        // Oscillate the background within the range
        float offsetX = Mathf.Sin(Time.time * speed) * movementRange;

        // Update the background's position
        transform.position = new Vector3(startPosition.x + offsetX, startPosition.y, startPosition.z);
    }
}
