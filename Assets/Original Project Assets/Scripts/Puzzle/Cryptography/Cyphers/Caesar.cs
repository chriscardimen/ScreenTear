using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caesar : Cypher
{
    //Shifts will always be left 4
    private int shift = 4;
    public override void Encrypt()
    {
        Initialize();
        
        encrypted = "";
        foreach (char ch in solution)
        {
            if (ch == 32)
            {
                encrypted += " ";
                continue;
            }
            
            int charToAdd = ch + shift;
            if (charToAdd > 122)
            {
                charToAdd = 96 + (charToAdd - 122);
            }
            encrypted += (Convert.ToChar(charToAdd));
        }
    }
}
