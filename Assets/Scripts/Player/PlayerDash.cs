using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private Rigidbody2D rb;
    private PlayerMovement playerMovement; 
    private bool isDashing = false;
    private bool canDash = true;
    private float dashEndTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if ((playerMovement.keyToggle && Input.GetKeyDown(KeyCode.J)) || (!playerMovement.keyToggle && Input.GetKeyDown(KeyCode.C)))
        {
            if (canDash)
                StartDash();
        }

        if (isDashing && Time.time >= dashEndTime)
        {
            StopDash();
        }
    }

    void StartDash()
    {
        isDashing = true;
        canDash = false;
        dashEndTime = Time.time + dashDuration;

        float dashDirection = playerMovement.isFacingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(dashDirection * dashSpeed, 0f);
    }

    void StopDash()
    {
        isDashing = false;
        Invoke(nameof(ResetDash), dashCooldown);
    }

    void ResetDash()
    {
        canDash = true;
    }

    public bool IsDashing()
    {
        return isDashing;
    }
}