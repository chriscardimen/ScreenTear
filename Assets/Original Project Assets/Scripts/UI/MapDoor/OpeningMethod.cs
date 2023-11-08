using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Sirenix.OdinInspector;
using System;

[CreateAssetMenu(menuName = "Mapping/DoorOpeningMethod")]
public class OpeningMethod: ScriptableObject
{
    [Serializable]
    public class ResourceAllocation
    {   
        public int characterIdx = 0;

        public int resourceIdx = 0;

        public int resourceCost = 1;

        

        public void UseResource(int cost)
        {
            ResourceManager.instance.RemoveResouce(characterIdx, resourceIdx, cost);
        }

    }


    public List<ResourceAllocation> neededResources;


    public bool isPuzzle = false;
        
    public int puzzleType = 0;

    ///ADD CLASSES FOR RESOURCES TO BE POTENTIALLY REMOVED
    public string methodName = "newMethod";

    public int timeRequired = 0;


    public void SelectOption()
    {

    }

}
