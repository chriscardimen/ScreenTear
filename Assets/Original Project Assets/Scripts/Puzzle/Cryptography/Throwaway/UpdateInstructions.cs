using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateInstructions : MonoBehaviour
{
    public CypherManagement cypher;
    public GameObject caesarInstructions, atBashInstructions;
    public ScrollRect scrollRect;
    public Image handle;
    
    
    void Update()
    {
        if (cypher.GetCypherName().Equals("Caesar"))
        {
            scrollRect.vertical = true;
            scrollRect.verticalScrollbar.enabled = true;
            handle.enabled = true;
            caesarInstructions.SetActive(true);
            atBashInstructions.SetActive(false);
        }
        else
        {
            scrollRect.vertical = false;
            scrollRect.verticalScrollbar.enabled = false;
            scrollRect.verticalScrollbar.value = 1;
            handle.enabled = false;
            caesarInstructions.SetActive(false);
            atBashInstructions.SetActive(true);
        }
    }
}
