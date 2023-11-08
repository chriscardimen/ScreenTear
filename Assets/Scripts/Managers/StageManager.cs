using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }

    public void GoToLevel(int buildIndex)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("User Interface");
        SceneManager.LoadScene(buildIndex, LoadSceneMode.Additive);
    }

    public void LoadNextLevel(bool isCutscene)
    {
        Time.timeScale = 1;
        if (isCutscene)
        {
            SceneManager.UnloadSceneAsync("User Interface");
        }
        int levelToLoad = SceneManager.GetActiveScene().buildIndex;
        if (!isCutscene)
        {
            SceneManager.LoadScene("User Interface");
            SceneManager.LoadScene(levelToLoad + 1, LoadSceneMode.Additive);
        }
        else
        {
            SceneManager.LoadScene(levelToLoad + 1);
        }
    }

    public void RetryLevel()
    {
        Time.timeScale = 1;
        SceneManager.UnloadSceneAsync("User Interface");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
