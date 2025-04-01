using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Echo : MonoBehaviour
{
    private Transform echoTransform;
    private float rangeMax = 5f;
    private float rangeSpeed = 5f; // Increased for better effect
    private List<Collider2D> alreadyPingedColliderList;
    private bool isPinging = false; // Prevents spamming
    private SpriteRenderer echoRenderer;

    private void Start()
    {
        echoTransform = transform.Find("EchoRange");
        alreadyPingedColliderList = new List<Collider2D>();
        echoRenderer = echoTransform.GetComponent<SpriteRenderer>();
        echoRenderer.enabled = false;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isPinging)
        {
            StartCoroutine(Ping());
        }
    }

    private IEnumerator Ping()
    {
        isPinging = true;
        float range = 0f;
        echoRenderer.enabled = true;

        while (range < rangeMax)
        {
            range += rangeSpeed * Time.deltaTime;
            echoTransform.localScale = new Vector2(range, range);

            // Detect platforms within range
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, range / 2f, Vector2.zero);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.collider.CompareTag("Ground"))
                {
                    if (!alreadyPingedColliderList.Contains(hit.collider))
                    {
                        alreadyPingedColliderList.Add(hit.collider);
                        Debug.Log("Detected: " + hit.collider.gameObject.name);

                        Platform platform = hit.collider.GetComponent<Platform>();
                        if (platform != null)
                        {
                            platform.AppearTemporarily();
                        }
                    }
                }
            }

            yield return null; // Wait until the next frame before continuing
        }

        // Reset for next use
        yield return new WaitForSeconds(0.5f); // Short delay before allowing another ping
        echoRenderer.enabled = false;
        alreadyPingedColliderList.Clear();
        isPinging = false;
    }
}
