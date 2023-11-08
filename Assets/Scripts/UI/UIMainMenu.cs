using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIMainMenu : MonoBehaviour
{
    public UIDocument doc;
    public VisualElement root, container;

    void Start()
    {
        root = doc.rootVisualElement;
        container = root.Q<VisualElement>("Container");
        container.style.display = DisplayStyle.Flex;
        container.visible = true;
    }
}
