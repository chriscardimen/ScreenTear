using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroNarration : MonoBehaviour
{
    public List<DialogueInstance> openingLines;
    
    void Start()
    {
        int index = 0;
        while (index < openingLines.Count)
        {
            DialogueManager.instance.AddToQueue(openingLines[index]);
            index++;
        }
    }

}
