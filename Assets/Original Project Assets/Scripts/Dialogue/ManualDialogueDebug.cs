using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Sirenix.OdinInspector;

public class ManualDialogueDebug : MonoBehaviour
{

    public DialogueInstance toAdd;

    // [Button]
    void AddThisDialogueToQueue()
    {
        DialogueManager.instance.AddToQueue(toAdd);
    }
}
