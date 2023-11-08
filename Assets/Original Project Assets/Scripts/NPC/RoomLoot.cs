using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomLootExample", menuName = "Loot/RoomLootObject", order = 1)]
public class RoomLoot : ScriptableObject
{
    public int characterIndex, resourceIndex, amt;
}
