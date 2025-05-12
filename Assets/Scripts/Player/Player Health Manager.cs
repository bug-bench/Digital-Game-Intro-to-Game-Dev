using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    public int maxHits = 2;
    private int currentHits = 0;

    private Vector3 checkpointPosition;

    private Rigidbody2D rb;
    private PlayerMovement movement;

    public DadActivation dadActivator; 

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();

        checkpointPosition = transform.position; // starting point as first checkpoint
    }

    public void SetCheckpoint(Vector3 position)
    {
        checkpointPosition = position;
        currentHits = 0; // Reset damage at checkpoint
        Debug.Log("Checkpoint set at: " + checkpointPosition);
    }

    public void TakeSpikeDamage(Vector3 spikePosition, float knockbackForce)
    {
        if (currentHits >= maxHits - 1)
        {
            Die();
        }
        else
        {
            currentHits++;
            Vector2 knockDir = (transform.position - spikePosition).normalized;
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(knockDir * knockbackForce, ForceMode2D.Impulse);
            Debug.Log("Player took spike damage! Hits: " + currentHits);
        }
    }

    public void TakeGasDamage(Vector3 gasSourcePosition, float knockbackForce)
    {
        Vector2 knockDir = (transform.position - gasSourcePosition).normalized;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(knockDir * knockbackForce, ForceMode2D.Impulse);
        Die();
    }

    private void Die()
    {
        Debug.Log("Player died. Respawning...");
        currentHits = 0;
        
        if (dadActivator != null)
        {
            dadActivator.ResetDad();
        }

        movement.SetInputEnabled(false);
        transform.position = checkpointPosition;
        rb.linearVelocity = Vector2.zero;
        Invoke(nameof(EnableInputAfterRespawn), 0.2f); // brief input lock
    }

    private void EnableInputAfterRespawn()
    {
        movement.SetInputEnabled(true);
    }
}