using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextDescription : MonoBehaviour
{
    private TextMeshProUGUI text;
    private Canvas textCanvas;
    
    private void Start()
    {
        textCanvas = GetComponent<Canvas>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        if (text.text.Equals(""))
        {
            text.text = transform.parent.gameObject.name;
            //Debug.Log($"No text set, defaulting to Object Name: {text.text}");
        }
    }

    void Update()
    {
        gameObject.transform.LookAt(SelectionManager.instance.gameObject.transform);
    }

    public void SetTextActive(bool isActive)
    {
        //Debug.Log($"Setting text {text.text} active: {isActive}");
        textCanvas.enabled = isActive;
    }
}
