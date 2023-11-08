using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Note : MonoBehaviour
{

    [InfoBox("@this.note")]

    [Button]
    void SetInfo(string info)
    {
        note = info.Replace("\\n", "\n");
    }
    [HideInInspector]
    public string note;
}
