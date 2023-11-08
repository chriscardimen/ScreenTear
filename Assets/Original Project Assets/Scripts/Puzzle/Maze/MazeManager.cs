using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{
    private bool hasWon;

    public bool CheckWin()
    {
        return hasWon;
    }

    public void SetWin(bool won)
    {
        hasWon = won;
    }
}
