using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCamera vCam;
    
    void Start()
    {
        
    }


    public void LockToObject()
    {
        Debug.Log("Locking to Object");
        foreach(var cam in Resources.FindObjectsOfTypeAll<CinemachineVirtualCamera>())
        {
            cam.Priority = 1;
        }
        vCam.enabled = true;
        vCam.Priority = 10;

    }
}
