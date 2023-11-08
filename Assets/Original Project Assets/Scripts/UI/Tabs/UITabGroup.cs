using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITabGroup : MonoBehaviour
{

    public List<UITabButton> tabButtons;

    public UITabButton curButton;

    public Sprite tabIdle, tabHover, tabSelected;


    public List<GameObject> objectsToSwap;

    private int _index = 0;

    public int GetCurTabIdx()
    {
        return _index;
    }

    public void Subscribe(UITabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<UITabButton>();
        }

        tabButtons.Add(button);
    }

    public void OnTabEnter(UITabButton button)
    {
        ResetTabs();
        button.background = tabHover;
    }

    public void OnTabExit(UITabButton button)
    {
        ResetTabs();
    }

    public void OnTabSelected(UITabButton button)
    {
        if (curButton != null)
        {
            curButton.Deselect();
        }
        
        curButton = button;
        curButton.Select();
        ResetTabs();
        button.background = tabSelected;
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < objectsToSwap.Count; i++)
        {
            if (i == index)
            {
                objectsToSwap[i].SetActive(true);

            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }
        _index = index;
    }




    public void ResetTabs()
    {
        foreach(UITabButton button in tabButtons)
        {
            if (curButton!=null && button == curButton)
                continue;
            button.background = tabIdle;
        }

    }



}
