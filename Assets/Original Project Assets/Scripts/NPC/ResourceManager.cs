using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public List<GameObject> characters;
    public List<GameObject> NPCDsiplay;

    public List<NPCInterface> interfaces;


    private static ResourceManager _instance;
    public static ResourceManager instance {get {return _instance;}}


    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.Log("Instance already made at - " + _instance.gameObject.name);
            Destroy(this.gameObject);
        } else {
            Debug.Log("Instance established in gameobject - " + gameObject.name);
            _instance = this;
        } 

        for(int i = 0; i < interfaces.Count; i++)
        {
            if (characters[i] == null)
            {
                Debug.LogWarning("not enogugh characters");
            }

            NPCDsiplay[i].GetComponentInChildren<Image>().sprite = characters[i].GetComponent<Character>().profilePic;

            interfaces[i].InitializeResources(characters[i]);   
        }
    }



    public void RemoveResouce(int characterIdx, int resourceIdx, int cost)
    {
        //Run cap of count
        
        DisplayResourceChange(characterIdx, resourceIdx, cost * -1);
        characters[characterIdx].GetComponents<ResourceInstance>()[resourceIdx].SubAmount(cost);
        // if ((characters[characterIdx].GetComponents<ResourceInstance>()[resourceIdx].currAmount) < 0)
        // {
        //     DisplayResourceChange(characterIdx, 0, -2);
        //     characters[characterIdx].GetComponents<ResourceInstance>()[resourceIdx].AddAmount(characters[characterIdx].GetComponents<ResourceInstance>()[resourceIdx].currAmount);
        //     characters[characterIdx].GetComponents<ResourceInstance>()[0].SubAmount(2);
        // }
    }

    public void DisplayResourceChange(int characterIdx, int resourceIdx, int change)
    {
        interfaces[characterIdx].ChangeResource(resourceIdx, change);
    }
}

