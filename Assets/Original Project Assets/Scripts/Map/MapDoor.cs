using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using Sirenix.OdinInspector;
// using Doozy.Engine.UI;
// using DG.Tweening;

public class MapDoor : MonoBehaviour
{
    public enum DoorState
    {
        unknown,
        seen,
        opened
    }

    [SerializeField] public DoorState doorState = DoorState.unknown;

    [SerializeField/*, ReadOnly*/] private MapRoom _room1;
    [SerializeField/*, ReadOnly*/] private MapRoom _room2;


    public List<OpeningMethod> openingMethods;


    private Image _doorImg;

    // private UIPopup _newPopup;


    void Start()
    {
        _doorImg = GetComponent<Image>();

        UpdateVisual();
    }

    public void SetRooms(MapRoom one, MapRoom two)
    {
        _room1 = one;
        _room2 = two;
    }


    public void UpdateVisual()
    {
        switch (doorState)
        {
            case DoorState.unknown:
                _doorImg.raycastTarget = false;
                // _doorImg.DOFade(0f, 0f);
                break;

            case DoorState.opened:
                _doorImg.raycastTarget = false;
                // _doorImg.DOFade(1f, 0.25f);
                break;

            case DoorState.seen:
                _doorImg.raycastTarget = true;
                // _doorImg.DOFade(0.5f, 0.25f);
                break;
        }
    }

    private void UpdateDoorState(DoorState newDoorState)
    {
        doorState = newDoorState;

        UpdateVisual();

    }

    public void ShowDoor()
    {
        if (doorState == DoorState.unknown)
        {
            UpdateDoorState(DoorState.seen);

        }
    }

    public void ShowOpeningPopup()
    {
        if (doorState == DoorState.opened)
        {
            return;
        }

        // _newPopup = UIPopup.GetPopup("DoorPopup");

        // if (_newPopup == null)
        //     return;

        // _newPopup.Show();


        MapDoorDisplay newDisplay;


        if (GameObject.Find("DoorPopup(Clone)").transform.GetChild(1).gameObject.TryGetComponent(out newDisplay))
            newDisplay.GeneratePopupInfo(this);
        else
        {
        }
    }

    public void SelectOption(int option)
    {
        Debug.Log("Option Selected - " + option.ToString());
        GameObject.FindWithTag("UITabArea").GetComponent<UITabGroup>().objectsToSwap[0].SetActive(true);
        //GameObject.FindWithTag("UITabArea").GetComponent<UITabGroup>().tabButtons[0].Select();
        GameObject.FindWithTag("UITabArea").GetComponent<UITabGroup>().objectsToSwap[1].SetActive(false);
        foreach (OpeningMethod.ResourceAllocation r in openingMethods[option].neededResources)
        {
            r.UseResource(r.resourceCost);
        }

        

        // if (_newPopup != null)
        // {
        //     _newPopup.Hide();
        // }

        

        // _newPopup = null;

        //Invoke puzzle 
        if (openingMethods[option].isPuzzle)
        {

            if (openingMethods[option].puzzleType == 0)
            {
                // UIPopup.GetPopup("Cypher Popup").Show();
            }
            else if (openingMethods[option].puzzleType == 1)
            {
                // UIPopup.GetPopup("MazePopup").Show();
                GameObject.FindGameObjectWithTag("MazeKeeper").GetComponent<MazeLoader>().NewMaze();
            }
        }

        doorState = DoorState.opened;
        
        UpdateVisual();
    }


    void SetToScouted(MapRoom room)
    {
        if (room.curState == MapRoom.RoomState.unknown)
        {
            room.curState = MapRoom.RoomState.scouted;
            room.UpdateVisual();
        }
    }

    public void SetRoomsToScouted()
    {
        SetToScouted(_room1);
        SetToScouted(_room2);
    }
}