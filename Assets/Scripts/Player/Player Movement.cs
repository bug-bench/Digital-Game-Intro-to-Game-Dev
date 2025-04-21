using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float acceleration = 10f;
    public float deceleration = 10f;
    public float airControlFactor = 0.8f;

    [Header("Jumping Settings")]
    public float jumpForce = 12f;
    public float maxJumpTime = 0.2f;
    public float jumpCutMultiplier = 0.5f;
    public float coyoteTime = 0.1f;
    public float jumpBufferTime = 0.1f;
    public float speedBoostFactor = 1.2f;

    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    [Header("Ground Check Settings")]
    public Transform groundCheck;
    public Transform spriteTransform;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;
    private bool isGrounded;

    [Header("Control Scheme")]
    public bool keyToggle = false;

    private Rigidbody2D rb;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private float inputX;
    private bool isJumping;
    private float jumpTimeCounter;
    private bool isDashing = false;
    private bool canDash = true;
    private float dashEndTime;
    public bool isFacingRight = true; 

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        HandleInput();
        HandleCoyoteTime();
        HandleJumpBuffer();
        HandleDash();
        CheckGrounded();
    }

    void FixedUpdate() {
        if (!isDashing) {
            ApplyMovement();
        }
    }

    void HandleInput() {
        inputX = 0;

        // Movement input
        if (keyToggle)
        {
            if (Input.GetKey(KeyCode.A)) inputX = -1;
            if (Input.GetKey(KeyCode.D)) inputX = 1;
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow)) inputX = -1;
            if (Input.GetKey(KeyCode.RightArrow)) inputX = 1;
        }

        // Jump input
        if ((keyToggle && Input.GetKeyDown(KeyCode.Space)) || (!keyToggle && Input.GetKeyDown(KeyCode.X)))
        {
            jumpBufferCounter = jumpBufferTime;
        }

        if ((keyToggle && Input.GetKeyUp(KeyCode.Space)) || (!keyToggle && Input.GetKeyUp(KeyCode.X)))
        {
            if (rb.linearVelocity.y > 0 && isJumping)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
                isJumping = false;
            }
        }
    }

    void HandleCoyoteTime() {
        if (isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;
    }

    void HandleJumpBuffer() {
        if (jumpBufferCounter > 0)
            jumpBufferCounter -= Time.deltaTime;
    }

    void HandleDash() {
        if ((keyToggle && Input.GetKeyDown(KeyCode.J)) || (!keyToggle && Input.GetKeyDown(KeyCode.C)))
        {
            if (canDash)
                Dash();
        }

        if (isDashing && Time.time >= dashEndTime)
        {
            StopDash();
        }
    }

    void ApplyMovement() {
        float targetSpeed = inputX * moveSpeed;
        float speedDiff = targetSpeed - rb.linearVelocity.x;
        float accelerationRate = isGrounded ? acceleration : acceleration * airControlFactor;
        float movement = speedDiff * accelerationRate * Time.fixedDeltaTime;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x + movement, rb.linearVelocity.y);

        if (!isGrounded)
        {
            rb.linearVelocity = new Vector2(Mathf.Lerp(rb.linearVelocity.x, targetSpeed, airControlFactor * Time.fixedDeltaTime), rb.linearVelocity.y);
        }
        
        if (inputX > 0 && !isFacingRight)
            Flip();
        else if (inputX < 0 && isFacingRight)
            Flip();
        
        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
        {
            Jump();
        }
    }

    void Jump() {
        jumpBufferCounter = 0;
        coyoteTimeCounter = 0;
        isJumping = true;
        jumpTimeCounter = maxJumpTime;

        if (Mathf.Abs(rb.linearVelocity.x) > moveSpeed * 0.8f)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x * speedBoostFactor, jumpForce);
        else
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    void Dash() {
        isDashing = true;
        canDash = false;
        dashEndTime = Time.time + dashDuration;
        rb.linearVelocity = new Vector2((inputX == 0 ? transform.localScale.x : inputX) * dashSpeed, 0);
    }

    void StopDash() {
        isDashing = false;
        Invoke("ResetDash", dashCooldown);
    }

    void ResetDash() {
        canDash = true;
    }

    void CheckGrounded() {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = spriteTransform.localScale;
        scale.x *= -1;
        spriteTransform.localScale = scale;
    }

    void OnDrawGizmosSelected() {
        if (groundCheck != null) {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Spike"))
        {
            ReflectionShield shield = GetComponent<ReflectionShield>();
            if (shield == null || !shield.IsShieldActive())
            {
                // 没开盾 → 玩家被弹飞
                Vector2 bounceDirection = (transform.position - collision.transform.position).normalized;
                rb.AddForce(bounceDirection * 1000f);
                Debug.Log("Hit spike! Knocked back!");
            }
            else
            {
                Debug.Log("Shield active → passed through spike safely.");
                // 什么都不做，玩家可以穿过
            }
        }
    }
}


