using System;
using System.Collections;
using System.Collections.Generic;
// using Doozy.Engine.UI;
using UnityEngine;

public abstract class Cypher : MonoBehaviour
{
    //Member variables
    private KeySelection keys;
    protected bool solved;
    protected string attempt, encrypted, solution;
    //Member functions
    public abstract void Encrypt();


    protected void Initialize()
    {
        keys = GameObject.FindWithTag("KeyManager").GetComponent<KeySelection>();
        attempt = "";
        solved = false;
        encrypted = "";
        solution = keys.GiveKey();
    }
    
    //Protected
    protected string ChooseKey()
    {
        return keys.GiveKey();
    }
    
    //Public
    public bool IsSolved()
    {
        return solved;
    }

    public void GetAttempt(string currAttempt)
    {
        StripAttempt(currAttempt);   
    }

    public void PrintKey()
    {
        Debug.Log(solution);
    }

    public string GetEncrypted()
    {
        return encrypted;
    }

    public string GetKey()
    {
        return solution;
    }

    public bool CheckSolution()
    {
        if (solution.Replace(" ", "").ToLower().Equals(attempt.ToLower().Replace(" ", "")))
        {

            // GameObject.Find("CypherPopup(Clone)").GetComponent<UIPopup>().Hide();
            solved = true;
            return true;
        }

        solved = false;
        return false;
    }

    public void PrintSolution()
    {
        Debug.Log(CheckSolution());
    }

    private void StripAttempt(string toStrip)
    {
        attempt = toStrip;
        attempt = attempt.Replace(" ", "");
        attempt = attempt.ToLower();
    }
}