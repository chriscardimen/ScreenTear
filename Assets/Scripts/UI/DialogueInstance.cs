using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using Doozy.Engine.UI;
// using Sirenix.OdinInspector;
// using Sirenix.Serialization;
using System;
// using Doozy.Engine;

[System.Serializable]
public class SingleVoiceLine
{
    

    [Multiline(5)]
    public string lineOfDialogue;
    public float overrideCPS = -1f;
    // public Sprite character;
    public Sprite characterImage;
    public float pause = 0f;
}


[CreateAssetMenu(menuName = "Dialogue/DialogueInstance")]
public class DialogueInstance:ScriptableObject
{



    public int priority = 1;

    public float overrideCPS = -1f;
    // public Sprite characterImage;
    public List<SingleVoiceLine> VoiceLines;
    // public Sprite character;
    public float initialPause = 0f;



    // [Button]
    public void AddToQueue()
    {
        DialogueManager.instance.AddToQueue(this);
    }
    
    public void AddDialogueToQueue(DialogueInstance dialogue)
    {
        DialogueManager.instance.AddToQueue(dialogue);
    }

}
