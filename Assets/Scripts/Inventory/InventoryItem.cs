using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;
using System;

[Serializable, CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]

public abstract class InventoryItem : ScriptableObject
{
    public string itemName = "Object";
    public string description = "An Object";
    public string checkID = "1234";

    public bool isConsumed = true;
    public int amount = 1;
    [AssetsOnly]
    public GameObject droppedPrefab;
    [AssetsOnly]
    public Texture2D itemImage;


    public abstract void PerformItemFunction(Survivor surv);
}


