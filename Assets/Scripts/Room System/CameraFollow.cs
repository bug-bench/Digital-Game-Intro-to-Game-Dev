using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothTime = 0.15f;
    public Vector2 offset = new Vector2(2f, 0f);

    [SerializeField] private float minX, maxX;
    [SerializeField] private float minY, maxY;
    private Vector3 velocity = Vector3.zero;

    void FixedUpdate()
    {
        if (player == null) return;

        float offsetX = player.localScale.x > 0 ? offset.x : -offset.x;
        Vector3 playerTarget = new Vector3(player.position.x + offsetX, player.position.y, 0);

        // Get the camera's visible size
        float camHalfHeight = Camera.main.orthographicSize;
        float camHalfWidth = camHalfHeight * Camera.main.aspect;

        // Clamp the target position to the room bounds, considering camera size
        float clampedX = Mathf.Clamp(playerTarget.x, minX + camHalfWidth, maxX - camHalfWidth);
        float clampedY = Mathf.Clamp(playerTarget.y, minY + camHalfHeight, maxY - camHalfHeight);

        Vector3 targetPosition = new Vector3(clampedX, clampedY, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }


    public void SetRoomBounds(RoomBounds bounds)
    {
        if (bounds == null)
        {
            Debug.LogWarning("CameraFollow: No RoomBounds given!");
            return;
        }

        minX = bounds.MinX;
        maxX = bounds.MaxX;
        minY = bounds.MinY;
        maxY = bounds.MaxY;

        Debug.Log($"CameraFollow updated bounds: minX={minX}, maxX={maxX}, minY={minY}, maxY={maxY}");
    }
}
