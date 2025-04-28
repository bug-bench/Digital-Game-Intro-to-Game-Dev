using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToLevelSelect : MonoBehaviour
{
    [Header("Scene Settings")]
    public string menuSceneName = "Level Select"; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            SceneManager.LoadScene(menuSceneName);
        }
    }
}