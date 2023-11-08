using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailFence : Cypher
{
    //Rails will always be 3
    private int rails = 3;
    
    public override void Encrypt()
    {
        Initialize();
        
        List<List<string>> railCategories = new List<List<string>>();
            
        int max_modifier = ((rails-1) * 2);
        List<int> spaces = new List<int>();
        for (int y = 0; y < solution.Length; y++)
        {
            if (solution[y] == ' ')
            {
                spaces.Add(y);
            }
        }
        
        for (int x = 0; x < rails; ++x)
        {
            int index = x;
            List<string> currCategory = new List<string>();    
            while (index < solution.Replace(" ", "").Length)
            {
                currCategory.Add(solution.Replace(" ", "")[index].ToString());
                index += max_modifier;
            }
            railCategories.Add(currCategory);
            max_modifier -= 2;
            if (max_modifier == 0)
            {
                max_modifier = ((rails-1) * 2);
            }
        }

        string putTogether = "";
        foreach (List<string> currSplit in railCategories)
        {
            foreach (string currChar in currSplit)
            {
                putTogether += currChar;
            }
        }
        
        
        foreach (int j in spaces)
        {
            putTogether = putTogether.Insert(j, " ");
        }
        encrypted = putTogether;
        
        //Debug.Log(encrypted);
    }
}
