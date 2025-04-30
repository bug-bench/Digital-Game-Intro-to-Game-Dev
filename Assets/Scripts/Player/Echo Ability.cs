using UnityEngine;
using System.Collections;

public class EchoAbility : MonoBehaviour
{
    public Animator playerAnimator;

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
            playerAnimator.SetBool("Echo", true);
        }
    }

    // Sends the Echo Ability as a raycast and visualises it
    void SendEcho()
    {
        playerAnimator.SetBool("Echo", false);

        Vector2 baseDirection = playerMovement != null && playerMovement.isFacingRight ? Vector2.right : Vector2.left;

        int rayCount = 30; // Number of rays to cast
        float fieldOfView = 90f; // Degrees of the cone
        float angleStep = fieldOfView / rayCount;
        float startAngle = baseDirection == Vector2.right ? -fieldOfView / 2f : 180f - fieldOfView / 2f;

        for (int i = 0; i <= rayCount; i++)
        {

            float angle = startAngle + i * angleStep;
            Vector2 dir = AngleToDirection(angle);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, echoRange, echoLayerMask);

            Debug.DrawRay(transform.position, dir * echoRange, Color.cyan, 0.5f);

            if (hit.collider != null)
            {
                EchoPlatform echoPlatform = hit.collider.GetComponent<EchoPlatform>();
                if (echoPlatform != null)
                {
                    echoPlatform.RevealPlatform();
                }
            }
        }

        // Optional: spawn the echo cone visual
        if (echoVisualPrefab != null)
        {
            GameObject echoEffect = Instantiate(echoVisualPrefab, transform.position, Quaternion.identity);
            Destroy(echoEffect, 0.5f);
        }
    }

    private Vector2 AngleToDirection(float angleDegrees)
    {
        float rad = angleDegrees * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }
}
