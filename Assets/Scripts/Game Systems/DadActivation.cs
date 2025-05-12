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

    private Vector3 originalPosition;

    void Start()
    {
        if (targetToActivate != null)
        {
            originalPosition = targetToActivate.transform.position;
        }
    }

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

    void Update()
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
                isMovingUp = false;
            }
        }
    }

    public void ResetDad()
    {
        triggered = false;
        isMovingUp = false;

        if (targetToActivate != null)
        {
            targetToActivate.transform.position = originalPosition;
        }
    }
}
