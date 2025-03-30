using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8f; // The maximum speed the player can move
    public float acceleration = 10f; // How quickly the player reaches full speed
    public float deceleration = 10f; // How quickly the player slows down when not pressing a direction
    public float airControlFactor = 0.8f; // Reduces movement control while in the air
    
    [Header("Jumping Settings")]
    public float jumpForce = 12f; // The strength of the jump
    public float maxJumpTime = 0.2f; // Maximum time the jump button can be held for a higher jump
    public float jumpCutMultiplier = 0.5f; // Reduces jump height if button is released early
    public float coyoteTime = 0.1f; // Time buffer that allows jumping just after leaving a ledge
    public float jumpBufferTime = 0.1f; // Time buffer that allows pressing jump slightly before landing
    public float speedBoostFactor = 1.2f; // Adds momentum to movement if jumping while running fast
    
    private Rigidbody2D rb;
    private bool isGrounded; // Checks if the player is touching the ground
    private float coyoteTimeCounter; // Timer for coyote time
    private float jumpBufferCounter; // Timer for jump buffer
    private float inputX; // Stores the left/right movement input
    private bool isJumping; // Checks if the player is currently jumping
    private float jumpTimeCounter; // Tracks how long the jump button has been held

    void Start() {
        rb = GetComponent<Rigidbody2D>(); 
    }

    void Update() {
        HandleInput(); 
        HandleCoyoteTime(); 
        HandleJumpBuffer(); 
    }

    void FixedUpdate() {
        ApplyMovement(); 
    }

    void HandleInput() {
        inputX = 0;
        if (Input.GetKey(KeyCode.LeftArrow)) inputX = -1;
        if (Input.GetKey(KeyCode.RightArrow)) inputX = 1;
        
        // Register jump input when X key is pressed
        if (Input.GetKeyDown(KeyCode.X)) {
            jumpBufferCounter = jumpBufferTime; // Start jump buffer timer
        }

        // If jump button is released early, reduce jump height
        if (Input.GetKeyUp(KeyCode.X) && isJumping) {
            if (rb.linearVelocity.y > 0) {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
            }
            isJumping = false;
        }
    }

    void HandleCoyoteTime() {
        // Reset coyote time when on the ground, otherwise decrease timer
        if (isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;
    }

    void HandleJumpBuffer() {
        // Decrease jump buffer timer over time
        if (jumpBufferCounter > 0)
            jumpBufferCounter -= Time.deltaTime;
    }

    void ApplyMovement() {
        // Calculate target speed based on input
        float targetSpeed = inputX * moveSpeed;
        float speedDiff = targetSpeed - rb.linearVelocity.x;
        float accelerationRate = isGrounded ? acceleration : acceleration * airControlFactor;
        float movement = speedDiff * accelerationRate * Time.fixedDeltaTime;
        
        // Apply movement to the player
        rb.linearVelocity = new Vector2(rb.linearVelocity.x + movement, rb.linearVelocity.y);
        
        // Allow some air control for smoother jumps
        if (!isGrounded) {
            rb.linearVelocity = new Vector2(Mathf.Lerp(rb.linearVelocity.x, targetSpeed, airControlFactor * Time.fixedDeltaTime), rb.linearVelocity.y);
        }
        
        // Handle jump execution when conditions are met
        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0) {
            Jump();
        }
    }

    void Jump() {
        jumpBufferCounter = 0; // Reset jump buffer timer
        coyoteTimeCounter = 0; // Reset coyote time counter
        isJumping = true; // Mark that the player is jumping
        jumpTimeCounter = maxJumpTime; // Start jump duration timer
        
        // If the player is moving fast, maintain momentum in jump
        if (Mathf.Abs(rb.linearVelocity.x) > moveSpeed * 0.8f) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x * speedBoostFactor, jumpForce);
        }
        else {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        // Check if player is touching the ground
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    void OnCollisionExit2D(Collision2D collision) {
        // Check if player leaves the ground
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }
}