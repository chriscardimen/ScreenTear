using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableTerminalCommands : MonoBehaviour
{
    public List<CommandAction> commandActionPairings;

    [Serializable]
    public class CommandAction{
        public string command;
        public UnityEvent action;
    }
}
