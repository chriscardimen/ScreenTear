using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConsumableItem : InventoryItem
{
    public float capacity = 100f;

    public float consumptionRate = 0.1f;

    public bool active = true;

    public UnityEvent<Survivor> inventoryUpdate;

    public override void PerformItemFunction(Survivor surv)
    {
        if (capacity > 0f && active)
            inventoryUpdate.Invoke(surv);
    }
}
