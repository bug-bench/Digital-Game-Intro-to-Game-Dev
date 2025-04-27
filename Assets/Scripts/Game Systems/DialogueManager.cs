using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public GameObject dialoguePanel;
    public TextMeshPro dialogueText;
    private SpriteRenderer panelRenderer;
    private float fadeDuration = 0.3f;

    private bool isFading = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        panelRenderer = dialoguePanel.GetComponent<SpriteRenderer>();
        dialoguePanel.SetActive(false);
    }

    public void ShowDialogue(string line)
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = line;
        StopAllCoroutines();
        StartCoroutine(FadeSpriteRenderer(1)); // Fade In
    }

    public void HideDialogue()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutAndDisable());
    }

    private IEnumerator FadeOutAndDisable()
    {
        yield return FadeSpriteRenderer(0); // Fade Out
        dialoguePanel.SetActive(false);
    }

    private IEnumerator FadeSpriteRenderer(float targetAlpha)
    {
        isFading = true;
        float startAlpha = panelRenderer.color.a;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            panelRenderer.color = new Color(panelRenderer.color.r, panelRenderer.color.g, panelRenderer.color.b, alpha);
            yield return null;
        }

        panelRenderer.color = new Color(panelRenderer.color.r, panelRenderer.color.g, panelRenderer.color.b, targetAlpha);
        isFading = false;
    }
}
