using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ReportEveryTimeKeyIsPressed : MonoBehaviour
{
    public KeyCode keyCode;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // This is just an example
        if (Input.GetKeyDown(keyCode))
        {
            // Generate A report
            WWWForm form = new WWWForm();
            form.AddField("keyCode", keyCode.ToString());
            form.AddField("Time.time", $"{Time.time:0.00}");
            form.AddField("udid",SystemInfo.deviceUniqueIdentifier);
            
            XnAnalytics.POST(form, ReportCallback);
        }
    }

    void ReportCallback(bool success, string note)
    {
        Debug.LogWarning($"ReportCallback: success: {success} Note: {note}");
    }
}
