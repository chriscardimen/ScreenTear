using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeEvent
{
    public int deadline = 0;

    public int initialDeadline = 0;
    
    UnityEvent successEffects;
    UnityEvent failureEffects;

    public TimeEvent(int timeRemaining)
    {
        deadline = timeRemaining;
        initialDeadline = timeRemaining;
    }

    public int DaysRemaining(){ return deadline/(24*60);}

    public int HoursRemaining(){ return (deadline/60)%24;}

    public int MinutesRemaining(){ return deadline%(24*60);}

    public void IncreaseTime(int mins)
    {
        deadline -= mins;
        if (mins <= 0)
        {
            if (failureEffects != null)
            {
                failureEffects.Invoke();
            }
        }
    }

    public void OnSuccess()
    {
        if (successEffects != null)
        {
            successEffects.Invoke();
        }
    }


}


public class TimeManager : MonoBehaviour
{

    //Singleton of TimeManager.
    private static TimeManager _instance;

    public static TimeManager instance {get {return _instance;}}

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

    //Refrences current day
    public int curDay = 0;

    public string GetDayStr()
    {
        return ("Day " + curDay.ToString());
    }

    public int curTime = 0;

    public string GetTimeStr()
    {
        return ((curTime/60).ToString() + ":" + (curTime%60).ToString("D2"));
    }


    private List<TimeEvent> events;

    public void AddTimeEvent(TimeEvent timeEvent)
    {
        if (events == null)
        {
            events = new List<TimeEvent>();
        }
        events.Add(timeEvent);
    }

    public void IncreaseTime(int mins)
    {
        
    }
}
