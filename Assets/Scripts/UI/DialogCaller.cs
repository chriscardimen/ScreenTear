using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogCaller : MonoBehaviour
{
    public void AddToDialogueQueue(DialogueInstance dialogue)
    {
        DialogueManager.instance.AddToQueue(dialogue);
    }

    public void AddToCutsceneDialogueQueue(DialogueInstance dialogue)
    {
        CutsceneDialogueManager.instance.AddToQueue(dialogue);
    }
}
