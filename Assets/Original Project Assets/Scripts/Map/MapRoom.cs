using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine.UI;
// using DG.Tweening;
using System.Linq;
// using Sirenix.Serialization;
using System;

public class ResourcePoint
{

}


public class MapRoom : MonoBehaviour
{
    public string _roomID;

    public string roomDescription;

    [SerializeField]
    public List<DialogueInstance> dialogueToShow;

    public List<RoomLoot> roomLoot;
    public enum RoomState {unknown, scouted, visited}

    public RoomState curState = RoomState.unknown;


    [SerializeField]
    float oxygenLevel = 21f;

    [SerializeField]
    float radiation = 0f;

    [SerializeField]
    float temperature = 20f;


    Image _roomImg;

    [SerializeField]
    List<MapDoor> _attachedDoors;

    [SerializeField]
    public Dictionary<MapRoom, List<MapDoor>> _RoomsToDoors;


    void Start()
    {
        _roomImg = GetComponent<Image>();


        UpdateVisual();

    }

    // [Button]
    public void ChangeRoomState(RoomState newState)
    {
        curState = newState;
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        switch (curState)
        {
            case RoomState.scouted:
                _roomImg.raycastTarget = true;
                // _roomImg.DOFade(0.5f, 0.25f);
                break;
            case RoomState.visited:
                _roomImg.raycastTarget = true;
                // _roomImg.DOFade(1f, 0.25f);
                break;
            case RoomState.unknown:
                _roomImg.raycastTarget = false;
                // _roomImg.DOFade(0f, 0f);
                break;
        }
    }

    public void AddDoor(MapDoor door, MapRoom otherRoom)
    {
        Debug.Log("Adding Door to " + gameObject.name);

        if (_attachedDoors == null)
        {
            _attachedDoors = new List<MapDoor>();
            
        }
        _attachedDoors.Add(door);

        if(_RoomsToDoors == null)
        {
            _RoomsToDoors = new Dictionary<MapRoom, List<MapDoor>>();
        }

        if(!_RoomsToDoors.ContainsKey(otherRoom))
        {
            _RoomsToDoors[otherRoom] = new List<MapDoor>();
        }

        _RoomsToDoors[otherRoom].Add(door);
    }

    public void VisitRoom()
    {
        EnvironmentManager.instance.ChangeEnvVars(temperature, radiation, oxygenLevel);

        ChangeRoomState(RoomState.visited);
        //DO NOT UNCOMMNET - Will cause cyclical loop
        //MapManager.instance.MoveToRoom(this);

        UpdateVisual();


        Debug.Log("Door amount : " + _attachedDoors.Count);
        foreach (MapDoor door in _attachedDoors) {door.ShowDoor();}

        foreach (RoomLoot r in roomLoot)
        {
            GameObject.FindWithTag("UITabArea").GetComponent<UITabGroup>().objectsToSwap[0].SetActive(true);
            //GameObject.FindWithTag("UITabArea").GetComponent<UITabGroup>().tabButtons[0].Select();
            GameObject.FindWithTag("UITabArea").GetComponent<UITabGroup>().objectsToSwap[1].SetActive(false);
            GameObject.FindWithTag("ResourceManager").GetComponent<ResourceManager>().DisplayResourceChange(r.characterIndex, r.resourceIndex, r.amt);
            ResourceManager.instance.characters[r.characterIndex].GetComponents<ResourceInstance>()[r.resourceIndex].AddAmount(r.amt);
            roomLoot.RemoveAt(0);
        }

        if (this.gameObject.tag == "BreakerRoom")
        {
            this.GetComponent<BreakerRoom>().isOn = true;
        }

    }

    public bool IsPathPossible(MapRoom otherRoom)
    {

        if (otherRoom == this)
        {
            return true;
        }

        if(_RoomsToDoors == null)
        {
            Debug.LogError("NO ROOM-TO-DOOR DICTIONARY");
            return false;
        }

        return (_RoomsToDoors.ContainsKey(otherRoom) && (_RoomsToDoors[otherRoom].Any(a => a.doorState == MapDoor.DoorState.opened)));
    }


    public Tuple<float, float, float> GetStats()
    {
        return new Tuple<float, float, float>(oxygenLevel, radiation, temperature);
    }



    
}
