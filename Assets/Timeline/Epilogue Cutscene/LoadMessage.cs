using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMessage : MonoBehaviour
{
    public GameObject LoadingMessage;

    // Start is called before the first frame update
    void Start()
    {
        LoadingMessage.SetActive(false);
    }

    public void SetMessageActive()
    {
        LoadingMessage.SetActive(true);
    }

}
