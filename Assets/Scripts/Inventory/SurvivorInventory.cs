using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;

[Serializable]
public class SurvivorInventory : MonoBehaviour
{
    [SerializeField] private int maxInventorySpaces = 2;
    [SerializeField] private int inventorySize = 2;
    [SerializeField] private List<InventoryItem> inventoryItems = new List<InventoryItem>();

    public bool AddToInventory(InventoryItem item)
    {
        // Debug.Log(inventoryItems);
        if (CheckInventorySize() < maxInventorySpaces)
        {
            Debug.Log("There is space in the inventory, adding " + item.itemName);
            if (!CheckForDuplicates(item))
            {
                GeneralInventoryItem toAdd = ScriptableObject.CreateInstance<GeneralInventoryItem>();

                toAdd.amount = item.amount;
                toAdd.itemName = item.itemName;
                toAdd.description = item.description;
                toAdd.checkID = item.checkID;
                toAdd.isConsumed = item.isConsumed;
                toAdd.droppedPrefab = item.droppedPrefab;
                toAdd.itemImage = item.itemImage;

                inventoryItems.Add(toAdd);

            }
            return true;
        }

        return false;
    }

    public bool CheckForDuplicates(InventoryItem newItem)
    {
        foreach (InventoryItem itm in inventoryItems)
        {
            if (itm.itemName.Equals(newItem.itemName))
            {
                itm.amount += newItem.amount;
                return true;
            }
        }
        return false;
    }

    public int CheckInventorySize()
    {
        int count = 0;
        foreach (InventoryItem itm in inventoryItems)
        {
            count += itm.amount;
        }
        return count;
    }

    public List<InventoryItem> GetInventoryItems()
    {
        return inventoryItems;
    }

    public void RemoveItem(InventoryItem item)
    {
        if (inventoryItems.Contains(item))
            inventoryItems.Remove(item);
    }

    public void RemoveItem(int slot)
    {
        if (inventoryItems.Count >= slot && inventoryItems[slot] != null)
            inventoryItems.RemoveAt(slot);
    }

    public void DropItem(int slot)
    {
        //Check if in range
        if (inventoryItems.Count < slot)
            return;

        //check if not null
        if (inventoryItems[slot] != null)
        {
            GameObject newDroppedItem = GameObject.Instantiate(inventoryItems[slot].droppedPrefab);
            newDroppedItem.GetComponent<InventoryInteractable>().itemToGive = ScriptableObject.Instantiate(inventoryItems[slot]);
            inventoryItems.RemoveAt(slot);

        }
    }
}