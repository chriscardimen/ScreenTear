using System;
using System.Collections;
using System.Collections.Generic;
// using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class WinUpdate : MonoBehaviour
{
    public MazeManager maze;
    public Sprite right, wrong;

    private void Start()
    {
        maze = GameObject.FindWithTag("MazeKeeper").GetComponent<MazeManager>();
    }

    private void Update()
    {
        if (maze.CheckWin())
        {
            this.gameObject.GetComponent<Image>().sprite = right;
            this.gameObject.GetComponent<Image>().color = Color.green;
            maze.SetWin(false);
            // GameObject.Find("MazePopup(Clone)").GetComponent<UIPopup>().Hide();
        }
        else
        {
            this.gameObject.GetComponent<Image>().sprite = wrong;
            this.gameObject.GetComponent<Image>().color = Color.red;
        }
    }
}