using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothTime = 0.15f;
    public Vector2 offset = new Vector2(2f, 0f);

    public float minX, maxX;
    public float minY, maxY;

    private Vector3 velocity = Vector3.zero;

    void FixedUpdate()
    {
        if (player == null) return;

        // Adjust horizontal offset based on player facing direction
        float offsetX = player.localScale.x > 0 ? offset.x : -offset.x;
        float targetX = Mathf.Clamp(player.position.x + offsetX, minX, maxX);
        float targetY = Mathf.Clamp(player.position.y, minY, maxY);

        Vector3 targetPosition = new Vector3(targetX, targetY, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    public void SetRoomBounds(RoomBounds bounds)
    {
        minX = bounds.minX;
        maxX = bounds.maxX;
        minY = bounds.minY;
        maxY = bounds.maxY;
    }
}
