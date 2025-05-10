using UnityEngine;

public class DadActivation : MonoBehaviour
{
    public GameObject targetToActivate;  // The object to activate and move
    public float delay = 5f;             // Delay before movement starts
    public float moveSpeed = 2f;         // How fast the object moves upward
    public float moveDistance = 10f;     // How far the object moves upward

    private bool triggered = false;
    private bool isMovingUp = false;
    private float startY;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(ActivateAfterDelay());
        }
    }

    private System.Collections.IEnumerator ActivateAfterDelay()
    {
        yield return new WaitForSeconds(delay);

        if (targetToActivate != null)
        {
            targetToActivate.SetActive(true);
            startY = targetToActivate.transform.position.y;
            isMovingUp = true;
        }
    }

    private void Update()
    {
        if (isMovingUp && targetToActivate != null)
        {
            Vector3 currentPosition = targetToActivate.transform.position;

            if (currentPosition.y < startY + moveDistance)
            {
                targetToActivate.transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            }
            else
            {
                isMovingUp = false; // Stop moving when target height is reached
            }
        }
    }
}