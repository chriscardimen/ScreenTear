using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiSceneLoader : MonoBehaviour
{
    public List<string> scenesToLoad;

    void Start()
    {
        foreach(string currScene in scenesToLoad){
            SceneManager.LoadScene(currScene, LoadSceneMode.Additive);
        }
    }

}
