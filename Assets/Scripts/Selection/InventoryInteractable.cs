using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class InventoryInteractable : Interactable
{
    public InventoryItem itemToGive;

    public UnityEvent onComplete;

    public override void OnInteraction(SurvivorController survivor)
    {
        base.OnInteraction(survivor);
        if (gameObject != null)
            StartCoroutine(AddToInventoryCoroutine(survivor));
    }

    IEnumerator AddToInventoryCoroutine(SurvivorController survivor)
    {
        //yield return new WaitWhile(survivor.isMoving);
        
        yield return new WaitWhile(() => (survivor.isMoving() || (Vector3.Distance(gameObject.transform.position, this.GoalLocation()) > this.interactDistance)));

        bool successfulAdd = survivor.data.inventory.AddToInventory(itemToGive);

        if (itemToGive.itemName == "OxygenMask")
        {
            survivor.data.wearingOxygenMask = true;
        }

        if (successfulAdd)
        {
            onComplete.Invoke();
            SFXManager.s.PlaySound(SFXManager.SFXCategory.PickupSound);
            Destroy(gameObject);
            base.StopAllCoroutines();
            yield return null;
        }
        else
        {
            OnInvalidInteraction();
        }
    }
}