using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Assign the player in the Inspector
    public float smoothTime = 0.15f; // Smoothing time for movement
    public Vector2 offset = new Vector2(2f, 0f); // Horizontal offset for off-center effect

    public float minX, maxX; // Level boundaries for X movement
    private float defaultY; // The default Y position of the camera
    private Vector3 velocity = Vector3.zero; // Used for SmoothDamp

    void Start()
    {
        if (player != null)
            defaultY = transform.position.y; // Store the initial Y position
    }

    void FixedUpdate()
    {
        if (player == null) return;

        // Adjust offset based on player direction
        float offsetX = player.localScale.x > 0 ? offset.x : -offset.x;

        // Follow X position while clamping within level bounds
        float targetX = Mathf.Clamp(player.position.x + offsetX, minX, maxX);

        // Follow Y position only if the player moves above the camera
        float targetY = player.position.y > transform.position.y ? player.position.y : defaultY;

        // Set the desired position
        Vector3 targetPosition = new Vector3(targetX, targetY, transform.position.z);

        // Smoothly move the camera to the target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}