using UnityEngine;

public class DadActivation : MonoBehaviour
{
    public GameObject doorObject;        // Add Boss Door fomr Hierarcy to Inspector
    public GameObject targetToActivate;  // Boss Sprite or Object
    public float delay = 5f;

    private bool triggered = false;

    void OnTriggerExit2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            Door door = doorObject.GetComponent<Door>();
            if (door != null)
            {
                triggered = true;
                StartCoroutine(ActivateAfterDelay());
            }
        }
    }

    private System.Collections.IEnumerator ActivateAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        if (targetToActivate != null)
            targetToActivate.SetActive(true);
    }
}
