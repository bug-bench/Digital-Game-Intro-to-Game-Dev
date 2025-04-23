using UnityEngine;

public class RoomBounds : MonoBehaviour
{
    public float minX, maxX;
    public float minY, maxY;

    // (Optional) Draw visual room bounds in Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 topLeft = new Vector3(minX, maxY, 0);
        Vector3 topRight = new Vector3(maxX, maxY, 0);
        Vector3 bottomLeft = new Vector3(minX, minY, 0);
        Vector3 bottomRight = new Vector3(maxX, minY, 0);

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}
