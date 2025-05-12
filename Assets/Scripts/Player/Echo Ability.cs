using UnityEngine;
using System.Collections;

public class EchoAbility : MonoBehaviour
{
    public Animator echoAnim;

    [Header("Echo Ability Settings")]
    [Header("Control Settings")]
    public bool keyToggle = false; // Toggle between Z and K
    public float echoRange = 5f;
    public LayerMask echoLayerMask;
    public GameObject echoVisual;
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

        Vector2 baseDirection = playerMovement != null && playerMovement.isFacingRight ? Vector2.right : Vector2.left;

        int rayCount = 30; // Number of rays to cast
        float fieldOfView = 90f; // Degrees of the cone
        float angleStep = fieldOfView / rayCount;
        float startAngle = baseDirection == Vector2.right ? -fieldOfView / 2f : 180f - fieldOfView / 2f;

        for (int i = 0; i <= rayCount; i++)
        {

            // Optional: spawn the echo cone visual
            if (echoVisual != null)
            {

                Vector2 offset = playerMovement != null && playerMovement.isFacingRight ? Vector2.right : Vector2.left;
                float spawnDistance = 0.5f; // Adjust this value to change how far in front it spawns
                Vector2 spawnPosition = (Vector2)transform.position + offset * spawnDistance;

                GameObject echoEffect = Instantiate(echoVisual, spawnPosition, Quaternion.identity);

                if (!playerMovement.isFacingRight)
                {
                    Vector3 scale = echoEffect.transform.localScale;
                    scale *= -1;
                    echoEffect.transform.localScale = scale;
                }

                Destroy(echoEffect, 0.5f);
            }

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
                CollisionEchoPlatform collisionEchoPlatform = hit.collider.GetComponent<CollisionEchoPlatform>();
                if (collisionEchoPlatform != null)
                {
                    collisionEchoPlatform.RevealPlatform();
                }
            }
        }
    }

    private Vector2 AngleToDirection(float angleDegrees)
    {
        float rad = angleDegrees * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }
}
