using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "Data", menuName = "Inventory/OxygenMask", order = 1)]
public class OxygenMask : InventoryItem
{
    public float oxygen = 100f;
    public override void PerformItemFunction(Survivor surv)
    {
        if (oxygen > 0)
        {
            Debug.Log("Oxygen depleting from Mask");
            oxygen -= surv.oxygenRate * Time.deltaTime;
        }
        else
        {
            Debug.Log("Mask is out of oxygen");
            //TODO: New sound effect for oxygen mask levels gone.
            surv.wearingOxygenMask = false;
            surv.inventory.RemoveItem(this);
        }
    }
}
