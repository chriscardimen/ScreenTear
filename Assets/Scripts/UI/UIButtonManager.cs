using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class UIButtonManager : MonoBehaviour
{
    public UIDocument doc;
    public Texture2D buttonActive, buttonInactive;
    private VisualElement root;
    private List<Button> buttonList;
    private List<VisualElement> displayList;
    private TextElement notificationCount;
    private ProgressBar powerMeter;
    private float initialTotalPower;

    void Start()
    {
        buttonList = new List<Button>();
        displayList = new List<VisualElement>();
        root = doc.rootVisualElement;
        Button dialogue = root.Q<Button>("DialogueButton");
        VisualElement dialogueDisplay = root.Q<VisualElement>("DialogueDisplay");
        Button objectives = root.Q<Button>("ObjectivesButton");
        VisualElement objectiveDisplay = root.Q<VisualElement>("ObjectiveDisplay");
        notificationCount = root.Q<TextElement>("NotificationCount");
        powerMeter = root.Q<ProgressBar>("GlobalPowerMeter");
        initialTotalPower = NewEnvironmentManager.instance.totalPower;
        dialogue.clicked += DialogueClick;
        objectives.clicked += ObjectivesClick;

        buttonList.Add(dialogue);
        buttonList.Add(objectives);
        displayList.Add(dialogueDisplay);
        displayList.Add(objectiveDisplay);

        SwitchTabs("DialogueButton");
    }

    void Update()
    {
        powerMeter.value = NewEnvironmentManager.instance.totalPower / initialTotalPower;
    }

    void DialogueClick()
    {
        SwitchTabs("DialogueButton");
        notificationCount.text = "0";
        notificationCount.visible = false;
    }

    void ObjectivesClick()
    {
        SwitchTabs("ObjectivesButton");
        if (Int32.Parse(notificationCount.text) > 0)
        {
            notificationCount.visible = true;
        }
    }

    void SwitchTabs(string buttonName)
    {
        for (int x = 0; x < buttonList.Count; x++)
        {
            if (!buttonName.Equals(buttonList[x].name))
            {
                buttonList[x].style.backgroundImage = buttonInactive;
                displayList[x].style.display = UnityEngine.UIElements.DisplayStyle.None;
            }
            else
            {
                buttonList[x].style.backgroundImage = buttonActive;
                displayList[x].style.display = UnityEngine.UIElements.DisplayStyle.Flex;

            }
        }
    }
}
