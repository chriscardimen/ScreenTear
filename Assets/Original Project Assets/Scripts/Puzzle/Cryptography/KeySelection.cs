using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class KeySelection : MonoBehaviour
{
    public List<string> keys;

    private void Start()
    {
        InitializeKeys();
    }

    private void InitializeKeys()
    {
        for (int x = 0; x < keys.Count; ++x)
        {
            //keys[x] = keys[x].Replace(" ", "");
            keys[x] = keys[x].ToLower();
        }
    }

    public string GiveKey()
    {
        string key = "";
        if (keys.Count > 0)
        {
            int rand = Random.Range(0, keys.Count);
            key = keys[rand];
            keys.RemoveAt(rand);
        }
        return key;
    }
}