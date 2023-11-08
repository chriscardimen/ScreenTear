using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Sirenix.OdinInspector;
using TMPro;

public class MapDoorDisplay : MonoBehaviour
{


    private MapDoor _relevantDoor;

    private List<OpeningMethod> methods;

    [SerializeField/*, Required, AssetsOnly*/]
    private GameObject _buttonPrefab;

    [SerializeField/*, Required*/]
    private GameObject _openingExample;

    public GameObject buttonParent;

    public Transform optionParent;


    public void GeneratePopupInfo(MapDoor door)
    {
        _relevantDoor = door;
        GeneratePopupInfo(door.openingMethods);
    }


    private int GetCurrentOptionIdx()
    {
        return buttonParent.GetComponent<UITabGroup>().GetCurTabIdx();
    }


    // Start is called before the first frame update
    public void GeneratePopupInfo(List<OpeningMethod> newMethods)
    {
        int index = 0;
        methods = newMethods;
        foreach (OpeningMethod method in methods)
        {
            //Add Button to Tab Group//
            GameObject newButton = Instantiate(_buttonPrefab, buttonParent.transform);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = method.methodName;
            newButton.GetComponentInChildren<TextMeshProUGUI>().fontSize = 50f;
            //newButton.GetComponentInChildren<TextMeshProUGUI>().font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            buttonParent.GetComponent<UITabGroup>().tabButtons.Add(newButton.GetComponent<UITabButton>());
            newButton.GetComponent<UITabButton>().tabGroup = buttonParent.GetComponent<UITabGroup>();


            //Add Content
            GameObject openingOption = Instantiate(_openingExample, optionParent);
            buttonParent.GetComponent<UITabGroup>().objectsToSwap.Add(openingOption);
            openingOption.GetComponent<MapDoorOptionGenerator>().UnpackOpeningMethod(method);
            if (index == 0)
            {
                openingOption.SetActive(true);
                index++;
            }
            else
            {
                openingOption.SetActive(false);
            }
            
            //If no current button set, set new one
            if(buttonParent.GetComponent<UITabGroup>().curButton == null)
                buttonParent.GetComponent<UITabGroup>().curButton = newButton.GetComponent<UITabButton>();

            
        }
        
    }

    public void SelectOption()
    {
        _relevantDoor.SelectOption(GetCurrentOptionIdx());

        if (_relevantDoor.doorState == MapDoor.DoorState.opened)
        {
            _relevantDoor.SetRoomsToScouted();
        }
        
    }

}
