using System.Collections;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Collider2D platformCollider;
    public float visibleDuration = 3f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        platformCollider = GetComponent<Collider2D>();

        HidePlatform(); // Hide it at start
    }

    public void AppearTemporarily()
    {
        ShowPlatform();
        Debug.Log(gameObject.name + " activated!");
        StopAllCoroutines();
        StartCoroutine(HideAfterDelay());
    }

    private void ShowPlatform()
    {
        spriteRenderer.enabled = true;
        //platformCollider.enabled = true;
    }

    private void HidePlatform()
    {
        spriteRenderer.enabled = false;
        //platformCollider.enabled = false;
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(visibleDuration);
        HidePlatform();
        Debug.Log(gameObject.name + " disappeared!");
    }
}
