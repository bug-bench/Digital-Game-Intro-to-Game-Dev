using UnityEngine;
using System.Collections;

public class CollisionEchoPlatform : MonoBehaviour
{
    [Header("Echo Platform Settings")]
    public GameObject echoPlatformOn;

    public float visibleDuration = 3f;

    private SpriteRenderer platformSpriteRenderer; // SpriteRenderer for the platform's visual representation

    // Initialise the platform's state (ensure it starts disabled)
    void Start()
    {
        if (echoPlatformOn != null)
        {
            platformSpriteRenderer = echoPlatformOn.GetComponent<SpriteRenderer>();

            // Disable the sprite renderer initially to prevent interactions
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
            if (platformSpriteRenderer != null)
            {
                platformSpriteRenderer.enabled = false;
            }
        }
    }
}
