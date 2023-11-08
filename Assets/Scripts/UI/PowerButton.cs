using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerButton : MonoBehaviour
{
    public RoomState roomState;

    public Canvas progressCanvas;

    private void Start()
    {
        progressCanvas = this.GetComponent<Canvas>();        
        progressCanvas.worldCamera = SelectionManager.instance.GetComponent<Camera>();
    }

    void Update()
    {
        this.gameObject.transform.forward = -1f * SelectionManager.instance.transform.forward;
    }


    public void TogglePower()
    {
        roomState.TogglePower();
    }
    
}
