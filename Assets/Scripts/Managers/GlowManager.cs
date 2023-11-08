using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowManager : MonoBehaviour
{
    public Material selected, nonselected;
    bool isGlowing = true;

    public void ToggleGlow()
    {
        if (isGlowing)
        {
            gameObject.GetComponent<MeshRenderer>().material = nonselected;
            isGlowing = false;
        } 
        else
        {
            gameObject.GetComponent<MeshRenderer>().material = selected;
            isGlowing = true;
        }
    }

}
