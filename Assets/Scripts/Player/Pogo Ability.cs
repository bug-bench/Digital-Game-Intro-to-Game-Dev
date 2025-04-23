using UnityEngine;

public class PogoAbility : MonoBehaviour
{
    [Header("Pogo Settings")]
    public float pogoBounceForce = 15f;         // Upward force applied when pogoing
    public float pogoCheckDistance = 2f;        // Distance to check below the player for pogoable surfaces
    public float pogoCooldown = 0.1f;           // Time between allowed pogo bounces

    [Header("Knockback Settings")]
    public float knockbackForceX = 12f;         // Horizontal force applied during knockback
    public float knockbackForceY = 10f;         // Vertical force applied during knockback
    public float knockbackCooldown = 0.2f;      // Time between allowed knockbacks

    private Rigidbody2D rb;
    private bool canPogo = false;               // Whether the player is currently above a pogoable object
    private bool recentlyPogoed = false;        // Flag to prevent pogo spam or repeated knockbacks
    private float lastPogoTime = 0f;            // Last time the player performed a pogo
    private float lastKnockbackTime = -999f;    // Last time knockback was applied

    private Transform playerTransform;
    private LayerMask pogoLayer;                // Layer mask to identify pogoable objects
    public bool keyToggle = true;              // Toggle between keybinds (K/Z)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = transform;
        pogoLayer = LayerMask.GetMask("Pogo Layer");
    }

    void Update()
    {
        CheckForPogoable(); // Check if there's a pogoable object below the player

        // Handle pogo input based on selected control scheme
        bool pogoPressed = (keyToggle && Input.GetKeyDown(KeyCode.K)) || (!keyToggle && Input.GetKeyDown(KeyCode.Z));

        // Execute pogo if input is pressed, player is falling, and cooldown has passed
        if (pogoPressed && canPogo && Time.time - lastPogoTime > pogoCooldown && rb.linearVelocity.y < 0)
        {
            DoPogoBounce();
        }
    }

    void CheckForPogoable()
    {
        // Cast a ray downward to check for pogoable objects within the specified distance
        RaycastHit2D hit = Physics2D.Raycast(playerTransform.position, Vector2.down, pogoCheckDistance, pogoLayer);
        Debug.DrawRay(playerTransform.position, Vector2.down * pogoCheckDistance, Color.red);

        canPogo = hit.collider != null;
    }

    void DoPogoBounce()
    {
        // Apply upward force and record time of pogo
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, pogoBounceForce);
        lastPogoTime = Time.time;
        recentlyPogoed = true;

        // Reset pogo flag after short delay
        Invoke(nameof(ResetPogoFlag), 0.1f);

        Debug.Log("POGO activated!");
    }

    void ResetPogoFlag()
    {
        // Allow new pogo or knockback after short cooldown
        recentlyPogoed = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if collided object is in the pogo layer
        if (((1 << collision.gameObject.layer) & pogoLayer) != 0)
        {
            // Apply knockback if recentlyPogoed is false and cooldown has passed
            if (!recentlyPogoed && Time.time - lastKnockbackTime > knockbackCooldown)
            {
                // Calculate knockback direction (away from object, biased upward)
                Vector2 knockDirection = new Vector2(
                    collision.contacts[0].normal.x != 0 ? collision.contacts[0].normal.x : -Mathf.Sign(transform.localScale.x),
                    1f
                ).normalized;

                // Apply knockback force
                rb.linearVelocity = new Vector2(knockDirection.x * knockbackForceX, knockDirection.y * knockbackForceY);
                lastKnockbackTime = Time.time;

                Debug.Log("Knockback applied from pogoable object!");
            }
        }
    }
}
