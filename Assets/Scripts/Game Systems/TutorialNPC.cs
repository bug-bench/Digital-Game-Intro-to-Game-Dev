using UnityEngine;

public class TutorialNPC : MonoBehaviour
{
    [Header("Dialogue Settings")]
    [TextArea(2, 4)]
    public string dialogueLine;

    private bool playerInRange = false;
    private DialogueManager dialogueManager;

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        if (dialogueManager == null)
            Debug.LogWarning("DialogueManager not found in scene!");
    }

    void Update()
    {
        if (playerInRange && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            if (dialogueManager != null)
                dialogueManager.ShowDialogue(dialogueLine);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (dialogueManager != null)
                dialogueManager.ShowDialogue(dialogueLine); // Speak immediately on entering
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (dialogueManager != null)
                dialogueManager.HideDialogue();
        }
    }
}
