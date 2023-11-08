using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateKey : MonoBehaviour
{
    public CypherManagement cypher;
    void Update()
    {
        if (cypher.IsSet())
        {
            this.gameObject.GetComponent<Text>().text = cypher.GetCypher().GetKey();
        }
    }
}
