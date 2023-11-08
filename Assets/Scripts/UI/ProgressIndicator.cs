using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressIndicator : MonoBehaviour
{
    public Canvas progressCanvas;

    private void Start()
    {
        progressCanvas = this.GetComponent<Canvas>();        
    }

    void Update()
    {
        ColorBlock updatedBlock = this.GetComponentInChildren<Slider>().colors;
        updatedBlock.normalColor = this.gameObject.GetComponent<Transform>().parent.GetComponentInChildren<MeshRenderer>().material.color;
        this.GetComponentInChildren<Slider>().colors = updatedBlock;
        this.gameObject.transform.LookAt(SelectionManager.instance.gameObject.transform);
    }
}
