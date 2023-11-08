using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateSolved : MonoBehaviour
{
    public CypherManagement cypher;
    public Sprite right, wrong;
    void Update()
    {
        if (cypher.IsSet())
        {
            if (cypher.GetCypher().IsSolved())
            {
                this.gameObject.GetComponent<Image>().sprite = right;
                this.gameObject.GetComponent<Image>().color = Color.green;
            }
            else
            {
                this.gameObject.GetComponent<Image>().sprite = wrong;
                this.gameObject.GetComponent<Image>().color = Color.red;
            }
        }
        else
        {
            this.gameObject.GetComponent<Image>().sprite = wrong;
            this.gameObject.GetComponent<Image>().color = Color.red;

        }
    }
}
