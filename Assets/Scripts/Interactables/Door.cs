using UnityEngine;

public class Door : MonoBehaviour
{
    [Tooltip("Target door this one connects to")]
    public Transform destination;

    private bool playerInRange = false;
    private Transform player;

    void Update()
    {
        if (playerInRange && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            if (player != null && destination != null)
            {
                // Move the player to the destination door
                player.position = destination.position;

                // Reset player velocity (if using Rigidbody)
                Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                if (rb != null)
                    rb.linearVelocity = Vector2.zero;

                // Update camera bounds based on new room
                RoomBounds newRoom = destination.GetComponentInParent<RoomBounds>();
                if (newRoom != null)
                {
                    CameraFollow cam = Camera.main.GetComponent<CameraFollow>();
                    if (cam != null)
                        cam.SetRoomBounds(newRoom);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
        }
    }
}
