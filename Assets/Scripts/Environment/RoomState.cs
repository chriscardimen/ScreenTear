using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

public class RoomState : MonoBehaviour
{
    // Start is called before the first frame update
    public string roomName;

    public float powerPerSecond = 0.01f;

    public float oxygen = 100f;

    public float maxOxygen = 100f;

    [SerializeField]
    bool unlimitedOxygen = false;

    public float passiveOxygenRecharge = 0.01f;

    public float poweredOxygenRecharge = 1f;


    private bool playedOxygenSound = false;

    [SerializeField, ReadOnly]
    public List<Survivor> survivorsInRoom;

    
    List<InventoryInteractable> itemsInRoom;

    void Start()
    {
        survivorsInRoom = new List<Survivor>();
        
        
    }



    public bool IsPowered()
    {
        return NewEnvironmentManager.instance.IsPoweredRoom(this);
    }

    private void ChangeOxygen(float value)
    {
    }

    private Color GetPowerColor()
    {
        if (!Application.isPlaying)
            return Color.gray;
        // Sirenix.Utilities.Editor.GUIHelper.RequestRepaint();
        return (IsPowered() ? Color.yellow : Color.gray);
    }

    [Button, DisableInEditorMode, GUIColor("GetPowerColor")]
    public void TogglePower()
    {
        if (NewEnvironmentManager.instance.IsPoweredRoom(this))
        {
            NewEnvironmentManager.instance.RemoveFromPoweredRooms(this);
        }
        else
        {

            NewEnvironmentManager.instance.AddRoomToPoweredRooms(this);
        }

        VisualRoomStats stat = GetComponentInChildren<VisualRoomStats>();
        if (stat != null)
        {
            stat.UpdatePowerMeter();
        }
        
    }
    
    public bool GetPower()
    {
        return NewEnvironmentManager.instance.IsPoweredRoom(this);
    }

    public void RemoveSurvivorFromRoom(Survivor surv)
    {
        survivorsInRoom.Remove(surv);
        if (survivorsInRoom.Count <= 0)
        {
            playedOxygenSound = false;
            //Debug.Log($"PlayedOxygenSound: {playedOxygenSound}");
        }
    }

    public void AddSurvivorToRoom(Survivor surv)
    {
        survivorsInRoom.Add(surv);
        AnalyticsManager.s.AddDataIntValue(
            roomName + AnalyticsManager.s.ROOM_ENTERED_STRING);
    }

    public bool DepleteOxygen(float rate)
    {
        if (unlimitedOxygen)
        {
            return true;
        }
        if (oxygen > 0)
        {
            oxygen -= (rate * Time.deltaTime);
        }
        if (oxygen <= 0f)
        {
            oxygen = 0f;
            //Debug.Log($"Oxygen in {roomName} has reached zero.");
            if (!playedOxygenSound)
            {
                Debug.Log("Should play Oxygen Sound here.");
                SFXManager.s.PlaySound(SFXManager.SFXCategory.OxygenLoss);
                playedOxygenSound = true;
            }
        }
        return (oxygen > 0f);
    }

    // Update is called once per frame
    void Update()
    {
        // Only restore oxygen if no survivors are in the room (If you want to change this decision,
        // let Aidan know. It might screw up the sound.)
        if (survivorsInRoom.Count <= 0)
        {
            oxygen = Mathf.Min(oxygen + (IsPowered() ? poweredOxygenRecharge : passiveOxygenRecharge) * Time.deltaTime, maxOxygen);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        SurvivorController controller = other.gameObject.GetComponent<SurvivorController>();
        if (controller != null)
        {

            controller.data.changeCurrentRoom(this);
        }
    }

}
