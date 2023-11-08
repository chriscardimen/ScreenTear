using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceInstance : MonoBehaviour
{
    public string resourceType;


    public Sprite resourceIcon;
    public int maxAmount, defaultIncrement, currAmount;

    private void Start()
    {
        defaultIncrement = 1;
        
    }

    public void SubAmount()
    {
        if (currAmount > 0)
        {
            currAmount -= defaultIncrement;
            if (currAmount < 0)
            {
                currAmount = 0;
            }
        }
    }
    
    public void SubAmount(int cost)
    {
        if (currAmount > 0)
        {
            currAmount -= cost;
            if (currAmount < 0)
            {
                currAmount = 0;
                
            }
        }
    }
    
    public void AddAmount()
    {
        if (currAmount < maxAmount)
        {
            currAmount += defaultIncrement;
            if (currAmount > maxAmount)
            {
                currAmount = maxAmount;
            }
        }
    }
    
    public void AddAmount(int amount)
    {
        if (currAmount < maxAmount)
        {
            currAmount += amount;
            if (currAmount > maxAmount)
            {
                currAmount = maxAmount;
            }
        }
    }

}