using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIObjectives : MonoBehaviour
{
    public UIDocument doc;
    private VisualElement root;
    TextElement objectiveTitle, objectiveDescription, subObjectiveDescriptions;
    VisualElement subObjectiveContainer;

    public string containerClass = "InformationStateContainer";
    public string subObjectiveNameClass = "InformationStateName";
    public string subObjectiveDescClass = "InformationStateDescription";
    public string subObjectiveNameClassCompleted = "InformationStateNameCompleted";
    public string subObjectiveDescClassCompleted = "InformationStateDescriptionCompleted";

    void Start()
    {
        root = doc.rootVisualElement;
        objectiveTitle = root.Q<TextElement>("ObjectiveTitle");
        objectiveDescription = root.Q<TextElement>("ObjectiveDescription");
        subObjectiveContainer = root.Q<VisualElement>("SubObjectiveContainer");
    }

    void Update()
    {
        if (!ObjectiveManager.instance.GetPrimaryObjective().goalCompleted)
        {
            objectiveTitle.text = ObjectiveManager.instance.GetPrimaryObjective().title;
            objectiveDescription.text = ObjectiveManager.instance.GetPrimaryObjective().description;
        }

        subObjectiveContainer.Clear();
        foreach (Objective obj in ObjectiveManager.instance.GetPrimaryObjective().subObjectives)
        {
            VisualElement container = new VisualElement();
            TextElement subObjName = new TextElement();
            TextElement subObjDesc = new TextElement();
            subObjName.text = obj.title;
            subObjDesc.text = obj.description;
            container.Add(subObjName);
            container.Add(subObjDesc);
            subObjectiveContainer.Add(container);
            container.AddToClassList(containerClass);

            if (!obj.goalCompleted)
            {
                subObjName.text = "☐ " + obj.title;
                subObjName.AddToClassList(subObjectiveNameClass);
                subObjDesc.AddToClassList(subObjectiveDescClass);
            }
            else
            {
                subObjName.text = "☑ " + obj.title;
                subObjName.AddToClassList(subObjectiveNameClassCompleted);
                subObjDesc.AddToClassList(subObjectiveDescClassCompleted);
            }

        }
        VisualElement spacer = new VisualElement();
        spacer.style.height = 45;
        subObjectiveContainer.Add(spacer);
    }
}
