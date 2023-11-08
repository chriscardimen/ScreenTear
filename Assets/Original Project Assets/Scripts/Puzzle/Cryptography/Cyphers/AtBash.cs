using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtBash : Cypher
{
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
            encrypted += (Convert.ToChar(122 - ch + 97));
        }
    }
}
