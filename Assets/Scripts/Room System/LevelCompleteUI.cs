using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteUI : MonoBehaviour
{
    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        
         if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
           SceneManager.LoadScene(nextSceneIndex);
          }
         else
        {
         Debug.Log("Last Level Back to MainMenu");
        SceneManager.LoadScene("Level Select");
        }

    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level Select");
    }
}
