using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingMessage : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 2;
    }


}
