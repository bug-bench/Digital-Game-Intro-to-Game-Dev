using UnityEngine;

public class ShedAbility : MonoBehaviour
{
    [Header("Shed Ability Settings")]
    public GameObject shedPrefab;
    public Transform spawnPoint;
    public int maxSheds = 3;
    public float shedLifetime = 5f;
    public LayerMask wallLayer;

    [Header("Wall Check Settings")]
    public Transform wallCheck;
    public float wallCheckRadius = 0.1f;
    private int currentShedCount = 0;
    private Rigidbody2D rb;
    private bool isStickingToWall = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CheckWallStick();

        if (isStickingToWall && Input.GetKeyDown(KeyCode.K) && currentShedCount < maxSheds)
        {
            CreateShed();
        }
    }

    void CheckWallStick()
    {
        if (wallCheck == null)
            return;

        bool touchingWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, wallLayer);

        if (touchingWall && Input.GetAxisRaw("Horizontal") != 0)
        {
            isStickingToWall = true;
        }
        else
        {
            if (isStickingToWall)
            {
                isStickingToWall = false;
            }
        }
    }

    void CreateShed()
    {
        if (shedPrefab == null)
        {
            Debug.LogWarning("No shedPrefab assigned to ShedAbility.");
            return;
        }

        GameObject shed = Instantiate(
            shedPrefab,
            spawnPoint != null ? spawnPoint.position : transform.position,
            Quaternion.identity
        );

        currentShedCount++;

        Destroy(shed, shedLifetime);
        StartCoroutine(ShedDespawnDelay(shedLifetime));
    }

    System.Collections.IEnumerator ShedDespawnDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentShedCount--;
    }

    void OnDrawGizmosSelected()
    {
        if (wallCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);
        }
    }
}