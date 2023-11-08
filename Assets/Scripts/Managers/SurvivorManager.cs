using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using UnityEngine.Analytics;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.UI;
using Object = UnityEngine.Object;

[Serializable]
public class Survivor
{
    public string m_Name = "Sample Sam";
    public float health = 100;
    public float maxHealth = 100;
    public float moveSpeed = 3.5f;
    public Slider progressBar;

    [SerializeField, Tooltip("Percent Move Speed If in an Unpowered Room"), MinMaxSlider(0f, 1f)]
    public float unpoweredMoveSpeedPenalty = 0.8f;
    public Texture2D survivorImage;
    public bool wearingOxygenMask;

    [Required, SceneObjectsOnly]
    public SurvivorController inGameController;

    [Required, SceneObjectsOnly]
    public DownedSurvivorInteractable downedSurvivorInteractable;
    
    public enum SurvivorState {healthy, wounded, downed};

    public RoomState currentRoom;
    public GeneralInteractable interactingWith;
    public float oxygenRate;

    [SerializeField]
    public SurvivorInventory inventory;

    public bool inPoweredRoom()
    {
        return currentRoom.IsPowered();
    }

    public SurvivorState currentState = SurvivorState.healthy;

    public void changeCurrentRoom(RoomState room)
    {
        if (currentRoom != null)
        {
            currentRoom.RemoveSurvivorFromRoom(this);
        }
        currentRoom = room;
        currentRoom.AddSurvivorToRoom(this);
    }
}

public class SurvivorManager : MonoBehaviour
{

    public List<Survivor> survivors;

//===SINGLETON CREATION===
    private static SurvivorManager _instance;
    public static SurvivorManager instance {get {return _instance;}}
    
    // Rate at which a survivor loses health from lack of oxygen
    [SerializeField]
    private float suffocationRate = 0.15f;
    
    public const string LOADER_NAME = "Loader";
    public const string SCOUT_NAME = "Scout";
    public const string ENGINEER_NAME = "Engineer";
    public const string HACKER_NAME = "Hacker";

    public Survivor GetSurvivor(string name)
    {
        foreach (Survivor surv in survivors)
        {
            if (surv.m_Name == name)
                return surv;
        }
        return null;
    }

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
    
    void Start()
    {
        foreach(Survivor surv in survivors)
        {
            surv.downedSurvivorInteractable.controller = surv.inGameController;
            surv.inGameController.data = surv;
            surv.downedSurvivorInteractable.enabled = false;
        }
    }

    //Heal or hurt survivor. Bool returns whether or not survivor is fully healed.
    public bool ChangeSurvivorHealth(Survivor surv, float amount)
    {
        surv.health += amount;
        if (surv.health <= 0 && amount < 0)
        {
            surv.health = 0;
            DownSurvivor(surv);
        }
        if (surv.health >= surv.maxHealth && surv.currentState != Survivor.SurvivorState.healthy)
        {
            surv.health = surv.maxHealth;
            PickupSurvivor(surv);
            return true;
        }

        return false;
    }

    //Change Survivor to downed state
    void DownSurvivor(Survivor surv)
    {


        DownAnalytics(surv);

        ///GAME OVER CODE HERE

        GameOver();

        ///////
    }

    void GameOver()
    {
        Time.timeScale = 0;
        GetComponent<InteractableTerminal>().OpenTerminal();


        //GameOverAnalytics(surv)
    }

    void DownAnalytics(Survivor surv)
    {
        AnalyticsManager.s.AddDataIntValue(surv.m_Name + AnalyticsManager.s.DOWNED_COUNT_STRING);
        AnalyticsManager.s.AddDataIntValue(AnalyticsManager.s.TOTAL_DOWNED_COUNT_STRING);
        AnalyticsManager.s.AddDataIntValue(surv.currentRoom.roomName + AnalyticsManager.s.DOWNED_COUNT_STRING);
    }

    //Return a Survivor to normal, active state
    void PickupSurvivor(Survivor surv)
    {
        surv.currentState = Survivor.SurvivorState.healthy;
        surv.downedSurvivorInteractable.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Survivor survivor in survivors)
        {
            //Get Current Room, deplete oxygen in room based off of survivor drain rate.
            //Check if they're wearing an oxygen mask.
            if (survivor.currentRoom != null && !survivor.wearingOxygenMask)
            {
                //If no oxygen, begin suffocation
                if(!survivor.currentRoom.DepleteOxygen(survivor.oxygenRate))
                {
                    ChangeSurvivorHealth(survivor, Time.deltaTime * -1 * suffocationRate);
                }
            }
            if (survivor.inventory != null)
                foreach(InventoryItem item in survivor.inventory.GetInventoryItems())
                {
                    item.PerformItemFunction(survivor);
                }
            
        }
    }
}
