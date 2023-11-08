using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UITransitionButtonManager : MonoBehaviour
{
    public UIDocument doc;
    private VisualElement root;
    private StageManager stageManager;

    void Start()
    {
        root = doc.rootVisualElement;
        stageManager = GetComponent<StageManager>();
        Button dialogue = root.Q<Button>("SkipCutscene");
        dialogue.clicked += DoSkipCutscene;
    }

    void DoSkipCutscene()
    {
        if (SceneManager.GetActiveScene().buildIndex < 11)
        {
            stageManager.LoadNextLevel(false);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 11)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 12)
        {
            SceneManager.LoadScene("Main Menu");
        }
    }


}
