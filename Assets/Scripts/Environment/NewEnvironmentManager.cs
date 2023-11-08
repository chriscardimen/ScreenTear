using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class NewEnvironmentManager : MonoBehaviour
{

    private static NewEnvironmentManager _instance;
    public static NewEnvironmentManager instance {get {return _instance;}}

    private bool outOfPower = false;
    
    public void Awake()
    {
        if (_instance != null && _instance != this)
        {
            //Debug.Log("Instance already made at - " + _instance.gameObject.name);
            Destroy(this.gameObject);
        } else {
            //Debug.Log("Instance established in gameobject - " + gameObject.name);
            _instance = this;
        }
    }

    [ReadOnly, ShowInInspector, HideInEditorMode]
    HashSet<RoomState> poweredRooms = new HashSet<RoomState>();

    [SerializeField]
    public float totalPower = 1000f;

    public bool AddRoomToPoweredRooms(RoomState room)
    {

        if (totalPower <= 0f)
            return false;

        if (poweredRooms.Contains(room))
            return false;
        else
        {
            poweredRooms.Add(room);
            return true;
        }
    }

    public bool RemoveFromPoweredRooms(RoomState room)
    {
        if (poweredRooms.Contains(room))
        {
            poweredRooms.Remove(room);
            return true;
        }
        else
            return false;
    }


    public bool IsPoweredRoom(RoomState room)
    {
        return poweredRooms.Contains(room);
    }



    // Start is called before the first frame update
    void Start()
    {
        poweredRooms = new HashSet<RoomState>();
    }


    void OnNoPower()
    {
        Debug.Log("No Power Remaining!");
        poweredRooms.Clear();

        SFXManager.s.PlaySound(SFXManager.SFXCategory.PowerLoss);
    }


    // Update is called once per frame
    void Update()
    {
        foreach(RoomState room in poweredRooms)
        {
            totalPower -= (Time.deltaTime * room.powerPerSecond);
        }

        if (totalPower <= 0f && !outOfPower)
        {
            OnNoPower();
            outOfPower = true;
        }
    }
}
