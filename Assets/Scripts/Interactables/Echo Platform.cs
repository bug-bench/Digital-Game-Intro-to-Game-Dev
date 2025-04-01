using UnityEngine;
using System.Collections;

public class EchoPlatform : MonoBehaviour
{
    [Header("Echo Platform Settings")]
    public GameObject echoPlatformOn;

    public float visibleDuration = 3f;

    private Collider2D platformCollider; // Collider for the platform
    private SpriteRenderer platformSpriteRenderer; // SpriteRenderer for the platform's visual representation

    // Initialise the platform's state (ensure it starts disabled)
    void Start()
    {
        if (echoPlatformOn != null)
        {
            echoPlatformOn.SetActive(false); // Initially hide the platform
            platformCollider = echoPlatformOn.GetComponent<Collider2D>();
            platformSpriteRenderer = echoPlatformOn.GetComponent<SpriteRenderer>();

            // Disable the collider and sprite renderer initially to prevent interactions
            if (platformCollider != null)
            {
                platformCollider.enabled = false;
            }

            if (platformSpriteRenderer != null)
            {
                platformSpriteRenderer.enabled = false;
            }
        }
    }

    // Reveals the platform (enables visibility and interaction)
    public void RevealPlatform()
    {
        Debug.Log("Revealing Echo Platform!"); // Debug: Confirm the platform is being revealed

        if (echoPlatformOn != null)
        {
            echoPlatformOn.SetActive(true); // Activate the platform's GameObject

            // Enable the collider and sprite renderer when the platform becomes visible
            if (platformCollider != null)
            {
                platformCollider.enabled = true;
            }

            if (platformSpriteRenderer != null)
            {
                platformSpriteRenderer.enabled = true;
            }

            // Start the coroutine to hide the platform after the specified duration
            StartCoroutine(HideAfterDelay());
        }
    }

    // Coroutine that hides the platform after a set duration
    IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(visibleDuration); // Wait for the platform to remain visible

        if (echoPlatformOn != null)
        {
            echoPlatformOn.SetActive(false); // Deactivate the platform's GameObject

            // Disable the collider and sprite renderer when the platform disappears
            if (platformCollider != null)
            {
                platformCollider.enabled = false;
            }

            if (platformSpriteRenderer != null)
            {
                platformSpriteRenderer.enabled = false;
            }
        }
    }
}
