using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Sirenix.OdinInspector;
using UnityEditor;
using System;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;
    public static MapManager instance {get {return _instance;}}

    // [Required, SceneObjectsOnly]
    public GameObject mapWindow;

    public GameObject mapDoors;

    public GameObject nav;

    public MapRoom currentRoom;



    ///DOOR DEFAULTS




    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.Log("Instance already made at - " + _instance.gameObject.name);
            Destroy(this.gameObject);
        } else {
            Debug.Log("Instance established in gameobject - " + gameObject.name);
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // [Button]
    void GenerateDoor(MapRoom a, MapRoom b, GameObject mapDoorPrefab)
    {
        // if (EditorApplication.isPlaying)
        // {
        //     Debug.LogWarning("DEV TOOL - DO NOT USE IN PLAY MODE");
        //     return;
        // }
        if (mapDoors == null)
        {
            mapDoors = new GameObject("Map Doors");
            mapDoors.transform.parent = mapWindow.transform;
        }

        GameObject newMapObj = Instantiate(mapDoorPrefab, mapDoors.transform);
        var newMapDoor = newMapObj.GetComponent<MapDoor>();
        a.AddDoor(newMapDoor, b);
        b.AddDoor(newMapDoor, a);
        newMapDoor.SetRooms(a, b);
        // newMapObj.transform.position = Vector3.Lerp(a.transform.position, b.transform.position, 0.5f);
    }



    public void MoveToRoom(MapRoom mapRoom)
    {
        if (currentRoom.IsPathPossible(mapRoom) && (mapRoom.curState != MapRoom.RoomState.unknown))
        {
            ///Execute code for moving rooms
            currentRoom = mapRoom;
            currentRoom.VisitRoom();
            // nav.transform.position = mapRoom.transform.position;
            
            int maxDialogues = mapRoom.dialogueToShow.Count;
            while (maxDialogues > 0)
            {
                DialogueManager.instance.AddToQueue(mapRoom.dialogueToShow[0]);
                mapRoom.dialogueToShow.RemoveAt(0);
                maxDialogues--;
            }
        }
        else
        {
            //Alert for unreachable room

            return;
        }
        Tuple<float, float, float> newstats = currentRoom.GetStats();
        EnvironmentManager.instance.ChangeEnvVars(newstats.Item3, newstats.Item2, newstats.Item1);
    }
}
