using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIConsoleManager : MonoBehaviour
{
    [Header("Properties")]
    public UIDocument doc;
    private ScrollView scrollView;
    private VisualElement root, menuContainer, inputContainer;
    private TextField input;

    [Header("Dialogue Instances")]
    public DialogueInstance help;
    public DialogueInstance invalidInput;
    public DialogueInstance credits;

    [Header("Levels")]
    public List<string> scenesToLoad;

    void Start()
    {
        root = doc.rootVisualElement;
        input = root.Q<TextField>("CommandInput");
        menuContainer = root.Q<VisualElement>("Container");
        scrollView = root.Q<ScrollView>("CommandOutput");
        scrollView.focusable = false;
        input.RegisterCallback<KeyDownEvent>(e => HandleInput(e));
        BlinkingCursor(input);
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

    //https://forum.unity.com/threads/how-do-you-detect-if-someone-clicks-enter-return-on-a-textfield.688579/
    void HandleInput(KeyDownEvent e)
    {
        if (doc != null)
        {
            if (input.focusController.focusedElement == input)
            {
                SFXManager.s.PlaySoundAtRandom(SFXManager.SFXCategory.SingleKeyClick);
            }
            if (e.keyCode == KeyCode.Return)
            {
                AddInput(input.text);
                input.value = "";
                input.focusController.focusedElement.Focus();
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
        HandleInput(input);
    }

    void HandleInput(string input)
    {
        string cleansedInput = input.ToLower().Replace(" ", "");

        if (cleansedInput.Equals("start"))
        {
            InitiateGame();
        }
        else if (cleansedInput.Equals("help"))
        {
            SendPrompt(help);
        }
        else if (cleansedInput.Contains("loadlevel"))
        {
            LoadLevel(cleansedInput);
        }
        else if (cleansedInput.Equals("mainmenu"))
        {
            ReturnToMenu();
        }
        else if (cleansedInput.Equals("exit"))
        {
            QuitGame();
        }
        else if (cleansedInput.Equals("credits"))
        {
            SendPrompt(credits);
        }
        else
        {
            SendPrompt(invalidInput);
        }
    }

    void QuitGame()
    {
        Application.Quit();
    }

    void LoadLevel(string cleansedInput)
    {
        string levelInput = cleansedInput.Substring(cleansedInput.LastIndexOf('l') + 1);
        try
        {
            int levelNum = Int32.Parse(levelInput);
            if (levelNum < scenesToLoad.Count)
            {
                ChooseLevel(levelNum);
            }
            else
            {
                SendPrompt(invalidInput);
            }
        }
        catch (FormatException)
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
        CommandManager.instance.AddToQueue(dialogue);
    }

    public void InitiateGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Level 0 Transition");
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1;
        int x = SceneManager.sceneCount;
        SceneManager.LoadScene("Main Menu");
    }

    public void ChooseLevel(int level)
    {
        switch (level)
        {
            case 0:
                InitiateGame();
                break;
            default:
                Time.timeScale = 1;
                SceneManager.LoadScene(scenesToLoad[level]);
                break;
        }
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
