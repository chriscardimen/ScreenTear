using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using NaughtyAttributes;
//using Sirenix.OdinInspector;

/// <summary>
/// Will allow tab and shift-tab to move between UI elements on children of this GameObject
/// Based on code from: https://forum.unity.com/threads/tab-between-input-fields.263779/
/// </summary>
public class XnTabBetweenChildUiElements : MonoBehaviour {
    /// <summary>
    /// This method shows up as a button if you have Sirenex OdinInspector or (possibly) the free NaughtyAttributes package
    /// </summary>
    //[NaughtyAttributes.Button(enabledMode: EButtonEnableMode.Editor)]
    [Button("Find Child UI Elements")]
    void FindChildUiElements() {
        if (uiElementOrder == null) uiElementOrder = new List<Selectable>();
        Selectable[] selectables = GetComponentsInChildren<Selectable>();
        foreach (Selectable sel in selectables) {
            if (!uiElementOrder.Contains(sel)) uiElementOrder.Add(sel);
        }
        Debug.Log($"Find Child UI Elements found {uiElementOrder.Count} elements that are children of {name}");
    }
    
    public List<Selectable> uiElementOrder;

    int uiIndex = -1;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            for (int i = 0; i < uiElementOrder.Count; i++) {
                if (uiElementOrder[i].gameObject.Equals(EventSystem.current.currentSelectedGameObject)) {
                    uiIndex = i;
                    break;
                }
            }

            if ((Input.GetKey(KeyCode.LeftShift)) || Input.GetKey(KeyCode.RightShift)) {
                uiIndex = uiIndex > 0 ? --uiIndex : uiIndex = uiElementOrder.Count - 1;
            } else {
                uiIndex = uiIndex < uiElementOrder.Count - 1 ? ++uiIndex : 0;
            }
            //uiElementOrder[uiIndex].Select();
        }
    }

    public void Select(int ndx = 0) {
       //uiElementOrder[ndx].Select();
    }

}

/* Original code from the post:

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
 
public class TabToNextGUIElement : MonoBehaviour
{
    public List<Selectable> elements;   // add UI elements in inspector in desired tabbing order
    int index;
 
    void Start()
    {
        index = -1;           // always leave at -1 initially
        //elements[0].Select(); // uncomment to have focus on first element in the list
    }
 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            for (int i = 0; i < elements.Count; i ++)
            {
                if (elements[i].gameObject.Equals(EventSystem.current.currentSelectedGameObject))
                {
                    index = i;
                    break;
                }
            }
 
            if (Input.GetKey(KeyCode.LeftShift))
            {
                index = index > 0 ? --index : index = elements.Count - 1;
            }
            else
            {
                index = index < elements.Count - 1 ? ++index : 0;
            }
            elements[index].Select();
        }
    }
}
 
*/