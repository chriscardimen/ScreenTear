using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TimelineManager : MonoBehaviour
{
    private PlayableDirector playableDirector;
    private StageManager stageManager;

    void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();
        stageManager = GetComponent<StageManager>();
    }

    void Update()
    {
        if (playableDirector.state != PlayState.Playing)
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                SceneManager.LoadScene("Main Menu");
            }
            else
            {
                stageManager.LoadNextLevel(false);
            }
        }
    }
}
