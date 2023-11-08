using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    //===SINGLETON CREATION===
    private static UIManager _instance;
    public static UIManager instance { get { return _instance; } }
    public UIPauseMenu pauseMenu;
    public UIDocument terminalPopup;
    public UIDocument gameDoc;

    public void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

}
