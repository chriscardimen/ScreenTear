using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeWin : MonoBehaviour
{
    private MazeManager manager;

    private void Start()
    {
        manager = GameObject.FindWithTag("MazeKeeper").GetComponent<MazeManager>();
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        manager.SetWin(true);
    }
}
