using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BreakerRoom : MonoBehaviour
{
    public bool isOn;
    public TextMeshProUGUI complete;
    private void Update()
    {
        if (isOn)
        {
            complete.text = "<b>Current Objective:</b>"
                + "\n\n"+

            "Description: Get to the breaker room and flip the switch, that should restore power to the facility. So that we can traverse the area easier."
                + "\n\n" +

                "Location: Breaker Room\n" +
            "Status: <color=green>Complete</color>";
        }
    }
}
