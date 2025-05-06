using TMPro;
using UnityEngine;

public class TutorialNPC : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public string[] dialogueLines;                  // Dialogue lines specific to this NPC
    public TextMeshPro dialogueText;                // Reference to TMP text object
    public float fadeSpeed = 2f;                    // Speed of fade in/out

    private int currentLine = 0;
    private bool playerNear = false;
    private bool textVisible = false;
    private bool isTyping = false;

    private Color originalColor;

    void Start()
    {
        originalColor = dialogueText.color;
        originalColor.a = 0f;
        dialogueText.color = originalColor;
        dialogueText.text = ""; // Start with blank text
    }

    void Update()
    {
        HandleFade();
        HandleInput();
    }

    void HandleFade()
    {
        Color currentColor = dialogueText.color;
        float targetAlpha = playerNear ? 1f : 0f;
        currentColor.a = Mathf.MoveTowards(currentColor.a, targetAlpha, fadeSpeed * Time.deltaTime);
        dialogueText.color = currentColor;
    }

    void HandleInput()
    {
        if (!playerNear) return;

        if (Input.GetKeyDown(KeyCode.W) && !isTyping)
        {
            if (currentLine < dialogueLines.Length)
            {
                dialogueText.text = dialogueLines[currentLine];
                currentLine++;
            }
            else
            {
                // End of dialogue
                dialogueText.text = "";
                currentLine = 0;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            currentLine = 0;
            dialogueText.text = dialogueLines.Length > 0 ? dialogueLines[0] : "";
            if (dialogueLines.Length > 1) currentLine = 1;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            dialogueText.text = "";
            currentLine = 0;
        }
    }
}
