using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Transform cameraTransform;  // Reference to the camera
    public float parallaxFactor;       // Speed factor for this layer
    public float textureUnitSizeX;     // Width of the texture (adjust manually or calculate)

    private Vector3 lastCameraPosition;

    void Start()
    {
        // Initialize the last camera position
        lastCameraPosition = cameraTransform.position;

        // Calculate texture unit size based on sprite renderer size
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            textureUnitSizeX = spriteRenderer.bounds.size.x;
        }
    }

    void Update()
    {
        // Calculate the camera movement
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        // Apply parallax effect
        transform.position += new Vector3(deltaMovement.x * parallaxFactor, deltaMovement.y * parallaxFactor, 0);

        // Infinite scrolling logic
        if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
        {
            float offset = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(cameraTransform.position.x + offset, transform.position.y, transform.position.z);
            Debug.Log("Offset: " + offset);
            Debug.Log("Camera X: " + cameraTransform.position.x);
            Debug.Log("Transform X: " + transform.position.x);
            Debug.Log("Texture Unit Size X: " + textureUnitSizeX);
        } else {
            Debug.Log("Camera is stationary");
        }

        // Update the last camera position
        lastCameraPosition = cameraTransform.position;
    }
}
