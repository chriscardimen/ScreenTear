using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

[Serializable, CreateAssetMenu(fileName = "Data", menuName = "Inventory/TestInventoryObject", order = 1)]
public class TestInventoryItem : InventoryItem
{
    public override void PerformItemFunction(Survivor surv)
    {
        // Debug.Log("Performing Item Function");
    }
}