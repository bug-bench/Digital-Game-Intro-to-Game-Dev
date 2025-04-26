using System.Collections;
using UnityEngine;

public class EchoAbility : MonoBehaviour
{
    [Header("Echo Ability Settings")]
    [Header("Control Settings")]
    public bool keyToggle = true; // Toggle between Z and K
    float echoRange = 5f;
    public LayerMask echoLayerMask;
    public GameObject echoVisualPrefab;
    private PlayerMovement playerMovement;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    // on start
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((keyToggle && Input.GetKeyDown(KeyCode.K)) || (!keyToggle && Input.GetKeyDown(KeyCode.Z)))
        {
            SendEcho();
        }
    }

    // Sends the Echo Ability as a raycast and visualizes it
    void SendEcho()
    {
        Vector2 direction = playerMovement != null && playerMovement.isFacingRight ? Vector2.right : Vector2.left;

        // Debugging raycasts
        Debug.DrawRay(transform.position, direction * echoRange, Color.cyan, 0.5f);

        // Ensure the mesh is reset or re-enabled
        if (meshFilter.mesh == null) meshFilter.mesh = new Mesh();

        // Reset or show the mesh renderer if it was disabled
        meshRenderer.enabled = true;

        // Setup for the mesh
        Mesh mesh = meshFilter.mesh;

        // Rotation and echo parameters
        transform.rotation = Quaternion.Euler(0f, 0f, 50f);
        float fov = 90f;
        Vector3 origin = Vector3.zero;
        int rayCount = 50;
        float angle = 0f;
        float angleIncrease = fov / rayCount;
        RaycastHit2D hit;

        // Allocate arrays for vertices and triangles
        Vector3[] vertices = new Vector3[rayCount + 1];
        int[] triangles = new int[rayCount * 3];

        // Set the origin vertex
        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;

        // Cast rays in a fan shape and collect vertices
        for (int i = 0; i < rayCount; i++)
        {
            Vector3 vertex;
            hit = Physics2D.Raycast(origin, GetVectorFromAngle(angle), echoRange, echoLayerMask);

            if (hit.collider != null)
            {
                vertex = hit.point;

                // Log hit information for debugging
                Debug.Log("Raycast hit: " + hit.collider.gameObject.name);

                // Reveal platform if an EchoPlatform is hit
                EchoPlatform echoPlatform = hit.collider.GetComponent<EchoPlatform>();
                if (echoPlatform != null)
                {
                    echoPlatform.RevealPlatform();
                    Debug.Log("Revealing platform: " + hit.collider.gameObject.name);
                }
                else
                {
                    Debug.Log("No EchoPlatform component found.");
                }
            }
            else
            {
                vertex = origin + GetVectorFromAngle(angle) * echoRange;
            }

            vertices[vertexIndex] = vertex;

            // Define triangles for the mesh
            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;
                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }

        // Assign mesh data
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Raycast in the facing direction (left or right)
        RaycastHit2D directionHit = Physics2D.Raycast(transform.position, direction, echoRange, echoLayerMask);

        Vector3 endPosition = directionHit.collider != null ? directionHit.point : (Vector3)(direction * echoRange) + transform.position;
        float distance = Vector3.Distance(transform.position, endPosition);

        // Instantiate and scale the echo visual effect
        if (echoVisualPrefab != null)
        {
            GameObject echoEffect = Instantiate(echoVisualPrefab, transform.position, Quaternion.identity);
            echoEffect.transform.localScale = new Vector3(distance, 1f, 1f);
            echoEffect.transform.position = (transform.position + endPosition);

            // Flip the effect if facing left
            if (direction == Vector2.left)
            {
                Vector3 scale = echoEffect.transform.localScale;
                scale.x *= -1;
                echoEffect.transform.localScale = scale;
            }

            // Destroy echo effect after a delay (3 seconds)
            Destroy(echoEffect, 3f);
        }

        // Optionally destroy or reset mesh after some time, to "hide" it (instead of destroying GameObject)
        StartCoroutine(HideMeshAfterDelay(0.5f));
    }

    // Coroutine to hide the mesh after a delay
    private IEnumerator HideMeshAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        meshRenderer.enabled = false;
    }

    // Logic to convert an angle to a vector direction
    static Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
}
