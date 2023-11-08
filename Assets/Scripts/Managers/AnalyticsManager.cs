using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
    Start with https://docs.unity.com/analytics/AnalyticsSDKGuide.html
    to create the initial setup for analytics
*/


public class AnalyticsManager : MonoBehaviour
{

    public static AnalyticsManager s; // singleton

    // the dictionary holding the data
    private Dictionary<string, object> DataToReport = new Dictionary<string, object>();

    // bool to decide if you want to report
    // might not want to report while debugging
    public bool ReportAnalytics = false;

    //====STRING CONSTANTS====
    [HideInInspector] public string LEVEL_NAME_STRING = "LevelName";
    [HideInInspector] public string DOWNED_COUNT_STRING = "Downed";
    [HideInInspector] public string TOTAL_DOWNED_COUNT_STRING = "TotalDowned";
    [HideInInspector] public string OBJECTIVE_COMPLETE_STRING = "Completed";
    [HideInInspector] public string INTERACTED_STRING = "Interacted";
    [HideInInspector] public string SELECTED_STRING = "Selected";
    [HideInInspector] public string IDLE_TIME_STRING = "IdleTime";
    [HideInInspector] public string ROOM_ENTERED_STRING = "Entered";
    [HideInInspector] public string MEDBAY_USED_STRING = "MedbayUsed";

    private void Awake()
    {
        // singleton code
        if (s == null)
        {
            s = this;
        }
        else if (s != this) 
        {
            s = this;
            Debug.LogWarning("Multiple AnalyticsManager scripts in scene."); 
        }

        DataToReport[LEVEL_NAME_STRING] = SceneManager.GetActiveScene().name;
    }

    public void AddDataIntValue(string valueName, int appendValue=1)
    {
        if (!DataToReport.ContainsKey(valueName))
        {
            DataToReport[valueName] = appendValue;
        }
        else
        {
            int currentValue = (int)DataToReport[valueName];
            DataToReport[valueName] = currentValue + appendValue;
        }

        if (!valueName.Contains(IDLE_TIME_STRING))
        {
            //Debug.Log(valueName + ": " + DataToReport[valueName]);
        }
    }
    public void AddDataFloatValue(string valueName, float appendValue)
    {
        if (!DataToReport.ContainsKey(valueName))
        {
            DataToReport[valueName] = appendValue;
        }
        else
        {
            float currentValue = Convert.ToSingle(DataToReport[valueName]);
            DataToReport[valueName] = currentValue + appendValue;
        }

        if (!valueName.Contains(IDLE_TIME_STRING))
        {
            Debug.Log(valueName + ": " + DataToReport[valueName]);
        }
    }  

    // called in other classes when you want to report the analytics
    public void ReportData()
    {
        if (ReportAnalytics)
        {
            WWWForm form = new WWWForm();
            foreach (var pair in DataToReport)
            {   
                form.AddField(pair.Key, $"{pair.Value}");
            }
            XnAnalytics.POST(form, ReportCallback);    
        }
        else
        {
            Debug.Log("Didn't work");
        }
    }
    
    void ReportCallback(bool success, string note)
    {
        Debug.LogWarning($"ReportCallback: success: {success} Note: {note}");
    }
}

/*
 * How we call the functions in other classes
 * In our tricks class we have these two functions that get called when the trick ends 
 * we create the dict first

    public virtual void CreateAnalytics()
    {
        string nameTimeString = trickName + " trick time";
        AnalyticsManager.s.AddToDictionary(nameTimeString, secondsTrickActive);
        string nameScoreString = trickName + " trick score";
        AnalyticsManager.s.AddToDictionary(nameScoreString, currentScore);
    }

 *  then we call report

    public virtual void ReportAnalytics()
    {
        string trickReportName = trickName + " report";
        AnalyticsManager.s.ReportData(trickReportName);
    }
 *
 * Please dm me if you have any questions
 */
