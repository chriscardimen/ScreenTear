using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateName : MonoBehaviour
{
    public CypherManagement cypher;
    void Update()
    {
        if (cypher.GetCypherName().Equals("Caesar"))
        {
            this.gameObject.GetComponent<TextMeshProUGUI>().color = Color.red;
        }
        else
        {
            this.gameObject.GetComponent<TextMeshProUGUI>().color = Color.green;
        }
        this.gameObject.GetComponent<TextMeshProUGUI>().text = cypher.GetCypherName();
    }
}
