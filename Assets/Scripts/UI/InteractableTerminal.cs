using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class InteractableTerminal : MonoBehaviour
{

    [Header("Properties")]
    private UIDocument doc;
    private ScrollView scrollView;
    private VisualElement root, popupContainer, inputContainer;
    private TextField commandInput;
    private PlayerInput input;
    public String consoleName;

    [Header("Dialogue Instances")]
    public DialogueInstance terminalInitialization;
    public DialogueInstance help;
    public DialogueInstance invalidInput;

    [Header("Custom Commands")]
    public List<CommandAction> commandActionPairings;

    [Serializable]
    public class CommandAction
    {
        public string command;
        public UnityEvent action;
    }



    void Start()
    {
        input = SelectionManager.instance.gameObject.GetComponent<PlayerInput>();
        doc = UIManager.instance.terminalPopup;
        root = doc.rootVisualElement;
        commandInput = root.Q<TextField>("CommandInput");
        popupContainer = root.Q<VisualElement>("Container");
        scrollView = root.Q<ScrollView>("CommandOutput");
        scrollView.focusable = false;
        BlinkingCursor(commandInput);
    }

    void SendNamePrompt(){
        DialogueInstance instance = ScriptableObject.CreateInstance<DialogueInstance>();
        SingleVoiceLine item = new SingleVoiceLine();
        List<SingleVoiceLine> item_list = new List<SingleVoiceLine>();
        item_list.Add(item);
        item.lineOfDialogue = ">Accessing " + consoleName + "...";
        instance.VoiceLines = item_list;
        AddToDialogueQueue(instance);
    }

    void LateUpdate()
    {
        inputContainer = root.Q<VisualElement>("unity-content-container");
        int childCount = inputContainer.childCount;
        if (childCount > 1)
        {
            scrollView.ScrollTo(inputContainer[childCount - 1]);
        }
    }

    public void PauseGame()
    {
        input.actions.Disable();
        SelectionManager.instance.gameObject.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
        input.actions.Enable();
        popupContainer.visible = true;
    }

    public void ResumeGame()
    {
        input.actions.Disable();
        SelectionManager.instance.gameObject.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        input.actions.Enable();
        popupContainer.visible = false;
        commandInput.UnregisterCallback<KeyDownEvent>(HandleEventInput);

    }

    public void OpenTerminal()
    {
        PauseGame();
        commandInput.RegisterCallback<KeyDownEvent>(HandleEventInput);
        scrollView.Clear();
        SendNamePrompt();
        SendPrompt(terminalInitialization);
        Debug.Log(this.gameObject.name);
        Debug.Log(commandActionPairings[0].action + " " + commandActionPairings[0].command);
    }

    //https://forum.unity.com/threads/how-do-you-detect-if-someone-clicks-enter-return-on-a-textfield.688579/
    void HandleEventInput(KeyDownEvent e)
    {
        Debug.Log("HandleEventInput: " + gameObject.name);
        if (doc != null)
        {
            if (commandInput.focusController.focusedElement == commandInput)
            {
                SFXManager.s.PlaySoundAtRandom(SFXManager.SFXCategory.SingleKeyClick);
            }
            if (e.keyCode == KeyCode.Return)
            {
                AddInput(commandInput.text);
                commandInput.value = "";
                commandInput.focusController.focusedElement.Focus();
            }
        }
    }

    void AddInput(string input)
    {
        DialogueInstance instance = ScriptableObject.CreateInstance<DialogueInstance>();
        SingleVoiceLine item = new SingleVoiceLine();
        List<SingleVoiceLine> item_list = new List<SingleVoiceLine>();
        item_list.Add(item);
        item.lineOfDialogue = "><color=#00ffffff>" + input + "</color>";
        instance.VoiceLines = item_list;
        AddToDialogueQueue(instance);
        HandleCommandInput(input);
    }

    void HandleCommandInput(string input)
    {
    
        string cleansedInput = input.ToLower().Replace(" ", "");

        if (cleansedInput.Equals("help"))
        {
            SendPrompt(help);
        }
        else if (cleansedInput.Equals("back"))
        {
            ResumeGame();
            
        }
        else if (commandActionPairings.Count > 0)
        {
            foreach (CommandAction pair in commandActionPairings)
            {
                if (cleansedInput.Equals(pair.command.ToLower().Replace(" ", "")))
                {
                    pair.action.Invoke();
                    commandActionPairings.Remove(pair);
                    ResumeGame();
                    break;
                }
            }
        }
        else
        {
            SendPrompt(invalidInput);
        }
    }

    void SendPrompt(DialogueInstance instance)
    {
        DialogueInstance prompt = ScriptableObject.Instantiate<DialogueInstance>(instance);
        AddToDialogueQueue(prompt);
    }

    public void AddToDialogueQueue(DialogueInstance dialogue)
    {
        InteractableTerminalManager.instance.AddToQueue(dialogue);
    }

    //https://forum.unity.com/threads/changing-caret-style-inside-a-textfield.1014244/
    public static void BlinkingCursor(TextField tf)
    {

        tf.schedule.Execute(() =>
        {
            if (tf.ClassListContains("transparentCursor"))
                tf.RemoveFromClassList("transparentCursor");
            else
                tf.AddToClassList("transparentCursor");
        }).Every(530);
    }

}
