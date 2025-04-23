using UnityEngine;

public class Door : MonoBehaviour
{
    [Tooltip("Target door this one connects to")]
    public Transform destination;

    [Tooltip("Tag used to identify the player")]
    public string playerTag = "Player";

    private bool playerInRange = false;
    private Transform player;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (player != null && destination != null)
            {
                player.position = destination.position;

                // Optional: Reset player velocity if using Rigidbody
                Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = Vector2.zero;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = true;
            player = other.transform;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = false;
            player = null;
        }
    }
}
