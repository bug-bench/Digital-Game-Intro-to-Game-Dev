using UnityEngine;
using System.Collections;

public class EchoAbility : MonoBehaviour
{
    [Header("Echo Ability Settings")]
    public float echoRange = 5f;
    public LayerMask echoLayerMask;
    public GameObject echoVisualPrefab;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SendEcho();
        }
    }

    // Sends the Echo Ability as a raycast and visualises it
    void SendEcho()
    {
        // Perform the raycast in the right direction with specified range and layer mask
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, echoRange, echoLayerMask);
        
        // Visualise the raycast in the scene view
        Debug.DrawRay(transform.position, Vector2.right * echoRange, Color.cyan, 0.5f);

        // If the raycast hits an object, process the hit object
        if (hit.collider != null)
        {
            Debug.Log("Raycast hit: " + hit.collider.gameObject.name); // Log the name of the hit object

            // Try to get the EchoPlatform component from the hit object
            EchoPlatform echoPlatform = hit.collider.GetComponent<EchoPlatform>();
            if (echoPlatform != null)
            {
                // If the hit object is an EchoPlatform, reveal it
                echoPlatform.RevealPlatform();
            }
        }
        else
        {
            Debug.Log("Raycast did not hit anything.");
        }

        // Determine the endpoint of the raycast
        Vector3 endPosition = hit.collider != null ? hit.point : transform.position + Vector3.right * echoRange;
        float distance = Vector3.Distance(transform.position, endPosition);

        // Instantiate a prefav represent the raycast in the scene
        if (echoVisualPrefab != null)
        {
            GameObject echoEffect = Instantiate(echoVisualPrefab, transform.position, Quaternion.identity);
            echoEffect.transform.localScale = new Vector3(distance, 0.1f, 1f); // Scale it to match the ray's length
            echoEffect.transform.position = (transform.position + endPosition) / 2; // Position it at the midpoint
            Destroy(echoEffect, 0.5f); // Destroy the visual effect after 0.5 seconds
        }
    }
}
