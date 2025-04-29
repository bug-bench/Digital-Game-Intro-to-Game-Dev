using UnityEngine;

public class TutorialPrompt : MonoBehaviour
{
    public GameObject promptUI;

    private void Start()
    {
        if (promptUI != null)
            promptUI.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (promptUI != null)
                promptUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (promptUI != null)
                promptUI.SetActive(false);
        }
    }
}
