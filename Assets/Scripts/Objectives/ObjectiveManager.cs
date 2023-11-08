using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

// [Serializable]
public class Objective
{

    public string title = "";
    public string description = "";


    [SerializeField]
    public bool hasGoal = true;

    [SerializeField, ShowIf("hasGoal")]
    public bool goalCompleted = false;

    [SerializeField]
    bool hiddenFromQuestLog = false;



    List<Interactable> requiredInteractables;

    [HideInInspector]
    public Objective parent;


    [SerializeField]
    public List<Objective> subObjectives;

    [SerializeField]
    UnityEvent onCompletionEvents;

    public void CheckComplete()
    {
        if (subObjectives != null)
        {
            foreach (Objective obj in subObjectives)
            {
                if (!obj.IsComplete())
                    return;
            }
        }



        if (hasGoal && !goalCompleted)
        {
            return;

        }

        onCompletionEvents.Invoke();
        if (parent != null)
        {
            parent.CheckComplete();

        }
        return;
    }

    public bool IsComplete()
    {
        if (subObjectives != null)
        {
            foreach (Objective obj in subObjectives)
            {
                if (!obj.IsComplete())
                {
                    return false;
                }
            }
        }



        if (hasGoal && !goalCompleted)
        {
            return false;
        }

        return true;
    }

    public void CompleteGoal()
    {
        if (hasGoal)
            goalCompleted = true;

        //CheckComplete();
    }

}

public class ObjectiveManager : SerializedMonoBehaviour
{
    //===SINGLETON CREATION===
    private static ObjectiveManager _instance;
    public static ObjectiveManager instance { get { return _instance; } }

    public void Awake()
    {
        if (_instance != null && _instance != this)
        {
            //Debug.Log("Instance already made at - " + _instance.gameObject.name);
            Destroy(this.gameObject);
        }
        else
        {
            //Debug.Log("Instance established in gameobject - " + gameObject.name);
            _instance = this;
        }
    }


    [OdinSerialize]
    Objective primaryObjective;
    public Objective GetPrimaryObjective()
    {
        return primaryObjective;
    }

    public void CheckPrimaryObjective()
    {
        GetPrimaryObjective().CheckComplete();
    }

    [OdinSerialize, ReadOnly]
    Dictionary<string, Objective> objDict;

    public void Start()
    {
        objDict = new Dictionary<string, Objective>();
        LinkObjectives(primaryObjective);
    }

    // public void Update()
    // {
    //     GetPrimaryObjective().CheckComplete();
    // }

    public void LinkObjectives(Objective obj)
    {
        objDict[obj.title] = obj;
        if ((obj.subObjectives != null) && (obj.subObjectives.Count > 0))
        {
            foreach (Objective obj2 in obj.subObjectives)
            {
                obj2.parent = obj;
                LinkObjectives(obj2);
            }
        }
    }

    public void CompleteObjective(string ObjTitle)
    {
        objDict[ObjTitle].CompleteGoal();
        CheckPrimaryObjective();
        AnalyticsManager.s.AddDataIntValue(
            ObjTitle + AnalyticsManager.s.OBJECTIVE_COMPLETE_STRING);
        //objDict[ObjTitle].CheckComplete();
    }
}
