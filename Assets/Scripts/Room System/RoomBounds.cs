using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RoomBounds : MonoBehaviour
{
    private BoxCollider2D roomCollider;

    private void Awake()
    {
        roomCollider = GetComponent<BoxCollider2D>();
    }

    public float MinX => GetWorldCenter().x - roomCollider.size.x * 0.5f;
    public float MaxX => GetWorldCenter().x + roomCollider.size.x * 0.5f;
    public float MinY => GetWorldCenter().y - roomCollider.size.y * 0.5f;
    public float MaxY => GetWorldCenter().y + roomCollider.size.y * 0.5f;

    private Vector2 GetWorldCenter()
    {
        return (Vector2)transform.position + roomCollider.offset;
    }

    void OnDrawGizmosSelected()
    {
        if (roomCollider == null) roomCollider = GetComponent<BoxCollider2D>();
        Gizmos.color = Color.cyan;
        Vector2 center = GetWorldCenter();
        Vector2 size = roomCollider.size;

        Vector3 topLeft = new Vector3(center.x - size.x / 2f, center.y + size.y / 2f, 0);
        Vector3 topRight = new Vector3(center.x + size.x / 2f, center.y + size.y / 2f, 0);
        Vector3 bottomLeft = new Vector3(center.x - size.x / 2f, center.y - size.y / 2f, 0);
        Vector3 bottomRight = new Vector3(center.x + size.x / 2f, center.y - size.y / 2f, 0);

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}
