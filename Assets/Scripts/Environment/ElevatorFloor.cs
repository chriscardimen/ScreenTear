using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ElevatorFloor : MonoBehaviour
{

    [Required, SerializeField, ChildGameObjectsOnly]
    private GameObject blocker;


    [InfoBox("Doors have not been implemented yet. Once Sebastian has implemented doors (with animation), add them to the prefab and link them here. \n -Dylan")]

    [SerializeField, ChildGameObjectsOnly]
    private GameObject door;

    public Transform destination;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseFloor()
    {
        blocker.SetActive(true);

        //Close Door Animation
    }

    public void OpenFloor()
    {
        blocker.SetActive(false);

    }


}
