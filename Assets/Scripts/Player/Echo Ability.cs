using UnityEngine;
using System.Collections;

public class EchoAbility : MonoBehaviour
{
    [Header("Echo Ability Settings")]
    [Header("Control Settings")]
    public bool keyToggle = false; // Toggle between Z and K
    public float echoRange = 5f;
    public LayerMask echoLayerMask;
    public GameObject echoVisualPrefab;
    private PlayerMovement playerMovement;
    
    // on start
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((keyToggle && Input.GetKeyDown(KeyCode.K)) || (!keyToggle && Input.GetKeyDown(KeyCode.Z)))
        {
            SendEcho();
        }
    }

    // Sends the Echo Ability as a raycast and visualises it
    void SendEcho()
    {
        Vector2 direction = playerMovement != null && playerMovement.isFacingRight ? Vector2.right : Vector2.left;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, echoRange, echoLayerMask);
        Debug.DrawRay(transform.position, direction * echoRange, Color.cyan, 0.5f);

        if (hit.collider != null)
        {
            Debug.Log("Raycast hit: " + hit.collider.gameObject.name);

            EchoPlatform echoPlatform = hit.collider.GetComponent<EchoPlatform>();
            if (echoPlatform != null)
            {
                echoPlatform.RevealPlatform();
            }
        }
        else
        {
            Debug.Log("Raycast did not hit anything.");
        }

        Vector3 endPosition = hit.collider != null ? hit.point : (Vector3)(direction * echoRange) + transform.position;
        float distance = Vector3.Distance(transform.position, endPosition);

        if (echoVisualPrefab != null)
        {
            GameObject echoEffect = Instantiate(echoVisualPrefab, transform.position, Quaternion.identity);
            echoEffect.transform.localScale = new Vector3(distance, 0.1f, 1f);
            echoEffect.transform.position = (transform.position + endPosition) / 2;

            // Flip the effect if facing left
            if (direction == Vector2.left)
            {
                Vector3 scale = echoEffect.transform.localScale;
                scale.x *= -1;
                echoEffect.transform.localScale = scale;
            }

            Destroy(echoEffect, 0.5f);
        }
    }
}
