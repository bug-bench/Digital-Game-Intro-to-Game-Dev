using UnityEngine;

public class LevelEndTrigger : MonoBehaviour
{
    public GameObject levelCompleteUI;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            levelCompleteUI.SetActive(true);
            Time.timeScale = 0f; // stop the game
        }
    }
}
