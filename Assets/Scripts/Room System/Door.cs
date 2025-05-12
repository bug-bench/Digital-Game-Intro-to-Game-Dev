using UnityEngine;
using System.Collections; 

public class Door : MonoBehaviour
{
    public Transform destination;

    private bool playerInRange = false;
    private Transform player;
    private ScreenFader fader;
    public RoomBounds room;
    public Animator animator;

    private void Start()
    {
        fader = FindFirstObjectByType<ScreenFader>();
    }

    void Update()
    {
        if (playerInRange && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            if (player != null && destination != null && fader != null)
            {
                StartCoroutine(fader.FadeOutIn(() =>
                {
                    player.position = destination.position;

                    Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                    if (rb != null)
                        rb.linearVelocity = Vector2.zero;

                    // NEW: Directly use assigned RoomBounds
                    if (room != null)
                    {
                        CameraFollow cam = Camera.main.GetComponent<CameraFollow>();
                        if (cam != null)
                            cam.SetRoomBounds(room);
                    }
                    else
                    {
                        Debug.LogWarning("No RoomBounds assigned to this door!");
                    }
                }));
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            playerInRange = true;
            animator.SetTrigger("EnterDoor");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
            animator.ResetTrigger("EnterDoor");
        }
    }
}
