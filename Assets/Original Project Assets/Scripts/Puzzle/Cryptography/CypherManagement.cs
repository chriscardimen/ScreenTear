using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Doozy.Engine.UI;
// using Doozy.Engine.Settings;
using Random = UnityEngine.Random;

public class CypherManagement : MonoBehaviour
{
    private Cypher mainCypher;
    //public RailFence railFence;
    public AtBash atBash;
    public Caesar caesar;

    private int curr_cypher = -1;

    private void Start()
    {
        ChooseRandom();
    }

    public void SwitchCypher(int state)
    {
        curr_cypher = state;
        switch (curr_cypher)
        {
            case 1:
                //mainCypher = railFence;
                return;
            case 2:
                mainCypher = atBash;
                return;
            case 3:
                mainCypher = caesar;
                return;
        }
    }

    public void ChooseRandom()
    {
        int rand = Random.Range(2, 4);
        SwitchCypher(rand);
        mainCypher.Encrypt();
    }

    public Cypher GetCypher()
    {
        return mainCypher;
    }

    public string GetCypherName()
    {
        switch (curr_cypher)
        {
            case 2:
                return "AtBash";
            case 3:
                return "Caesar";
        }

        return "";
    }

    public bool IsSet()
    {
        if (curr_cypher != -1)
        {
            return true;
        }

        return false;
    }

    public void AttemptSolution(string input)
    {
        if (input != null)
        {
            mainCypher.GetAttempt(input);
            mainCypher.CheckSolution();
        }
    }
}