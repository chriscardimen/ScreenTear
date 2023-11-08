using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DownedSurvivorInteractable : Interactable
{
    [HideInInspector]
    public SurvivorController controller;
    
    public override void OnInteraction(SurvivorController survivor)
    {
        base.OnInteraction(survivor);
        // Adding a double check just in case. TURNS OUT I NEEDED IT. KEEP THIS HERE.
        if (controller.data.currentState == Survivor.SurvivorState.downed)
        {
            // This code is written under the assumption that the controller and this
            // interactable will be attached to the same unity object.
            survivor.survivorBeingCarried = controller.data.m_Name;
            gameObject.transform.parent = survivor.gameObject.transform;
            controller.data.inGameController.Follow(survivor.gameObject.transform);
        }
    }
}