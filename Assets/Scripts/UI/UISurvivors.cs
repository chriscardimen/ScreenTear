using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UISurvivors : MonoBehaviour
{
    public UIDocument doc;
    VisualElement root;

    void Start()
    {
        root = doc.rootVisualElement;
    }

    void Update()
    {
        List<Survivor> _survivors = SurvivorManager.instance.survivors;
        List<Survivor> selected = SelectionManager.instance.getSelectedSurvivors();

        for (int x = 0; x < _survivors.Count; x++)
        {
            VisualElement imageElement = root.Q<VisualElement>("Character" + x + "Image");
            TextElement nameElement = root.Q<TextElement>("Character" + x + "NameStats");
            TextElement itemElement = root.Q<TextElement>("Character" + x + "StatsDisplay");
            ProgressBar healthElement = root.Q<ProgressBar>("Character" + x + "HealthDisplay");
            itemElement.text = "";

            Survivor surv = _survivors[x];

            nameElement.text = surv.m_Name;
            imageElement.style.backgroundImage = surv.survivorImage;
            healthElement.value = (surv.health / surv.maxHealth);
            itemElement.text += "State: " + surv.currentState + "\n";
            itemElement.text += "Speed: " + GetSpeedAsIcon(surv.moveSpeed);

            if (selected.Contains(_survivors[x]))
            {
                SetBorderColors(nameElement, surv, false);
            }
            else
            {
                SetBorderColors(nameElement, surv, true);
            }

        }
    }

    public void SetBorderColors(VisualElement nameElement, Survivor surv, bool transparent)
    {
        if (transparent)
        {
            nameElement.style.borderBottomColor = new Color(0, 0, 0, 0);
        }
        else
        {
            nameElement.style.borderBottomColor = surv.progressBar.colors.normalColor;

        }
    }

    string GetSpeedAsIcon(float speed)
    {
        switch (speed)
        {
            case (1):
                return ">";
            case (2):
                return ">>";
            case (3):
                return ">>>";
            case (4):
                return ">>>>";
            case (5):
                return ">>>>>";
            default:
                return "-";
        }
    }
}
