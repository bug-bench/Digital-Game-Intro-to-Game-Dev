using UnityEngine;
using TMPro;
using System.Collections;

public class TutorialNPC : MonoBehaviour
{
    public GameObject spriteObject;             // needs a child object with the SpriteRenderer
    public Sprite injuredSprite;                // Sprite when NPC is injured
    public Sprite healedSprite;                 // Sprite after the player interacts with them - presses W

    public GameObject panelObject;               // Panel object
    public TextMeshPro textComponent;           // TextMeshPro component

    [TextArea] public string startingDialogue = "Help... I can't move..."; // Change in inspector if you want something different
    [TextArea] public string[] dialogueLines;   // Dialogue when player interacts
    private int currentLine = 0;

    private SpriteRenderer childSpriteRenderer;
    private bool playerInRange = false;
    private bool interacted = false;

    void Start()
    {
        if (spriteObject != null)
            childSpriteRenderer = spriteObject.GetComponent<SpriteRenderer>();

        if (childSpriteRenderer != null && injuredSprite != null)
            childSpriteRenderer.sprite = injuredSprite;

        if (panelObject != null && textComponent != null)
        {
            textComponent.text = startingDialogue;
            StartCoroutine(FadeTextIn(textComponent, 0.5f));
            StartCoroutine(FadeOutAfterDelay(4f));
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.W))
        {
            if (!interacted)
            {
                interacted = true;


                if (childSpriteRenderer != null && healedSprite != null)
                    childSpriteRenderer.sprite = healedSprite;

                currentLine = 0;
                ShowNextLine();
            }
            else
            {
                ShowNextLine();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    void ShowNextLine()
    {
        if (textComponent != null && currentLine < dialogueLines.Length)
        {
            StopAllCoroutines();
            textComponent.text = dialogueLines[currentLine];
            StartCoroutine(FadeTextIn(textComponent, 0.25f));
            currentLine++;
        }
        else
        {
            StartCoroutine(FadeTextOut(textComponent, 0.5f));
        }
    }

    IEnumerator FadeTextIn(TextMeshPro tmp, float duration)
    {
        float elapsed = 0f;
        Color c = tmp.color;
        c.a = 0f;
        tmp.color = c;
        panelObject.SetActive(true);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsed / duration);
            tmp.color = c;
            yield return null;
        }
    }

    IEnumerator FadeTextOut(TextMeshPro tmp, float duration)
    {
        float elapsed = 0f;
        Color c = tmp.color;
        c.a = 1f;
        tmp.color = c;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            c.a = 1f - Mathf.Clamp01(elapsed / duration);
            tmp.color = c;
            yield return null;
        }

        panelObject.SetActive(false);
    }

    IEnumerator FadeOutAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!interacted)
        {
            StartCoroutine(FadeTextOut(textComponent, 0.5f));
        }
    }
}