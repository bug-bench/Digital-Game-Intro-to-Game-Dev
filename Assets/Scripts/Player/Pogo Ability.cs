using UnityEngine;

public class PogoAbility : MonoBehaviour
{
    [Header("Pogo Settings")]
    public float pogoBounceForce = 15f;
    public float pogoCheckDistance = 0.5f;

    private Rigidbody2D rb;
    private bool canPogo = false;
    private Transform playerTransform;
    private LayerMask pogoLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = transform;

        pogoLayer = LayerMask.GetMask("Pogo Layer");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && canPogo)
        {
            DoPogoBounce();
        }

        CheckForPogoable();
    }

    void CheckForPogoable()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerTransform.position, Vector2.down, pogoCheckDistance, pogoLayer);
        Debug.DrawRay(playerTransform.position, Vector2.down * pogoCheckDistance, Color.red);

        if (hit.collider != null)
        {
            Debug.Log("Pogoable object detected: " + hit.collider.gameObject.name);
            canPogo = true;
        }
        else
        {
            canPogo = false;
        }
    }

    void DoPogoBounce()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, pogoBounceForce);
        canPogo = false;

        //Feedback
        Debug.Log("POGO activated!");
    }
}