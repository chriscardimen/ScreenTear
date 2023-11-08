using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIConnector : MonoBehaviour
{
    public void CallPauseMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            UIManager.instance.pauseMenu.TogglePause();
        }
    }
}
