using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Sirenix.OdinInspector;
//using UnityEditor.VersionControl;
using UnityEngine.AI;


public class Elevator : MonoBehaviour
{

    [SerializeField]

    List<ElevatorFloor> floorDoors;

    
    public float floorLevel = 0;

    [SerializeField, Required]
    private RoomState room;


    private int targetFloor = 0;

    private int prevFloor = -1;

    [SerializeField]
    private float maxSpeed = 1f;

    Vector3 vel;

    private Transform originalSurvivorTransform;

    private bool movedToFloorOnStart = false;

    // Start is called before the first frame update
    void Start()
    {
        MoveToFloor(targetFloor);
    }

    // Update is called once per frame
    void Update()
    {

        if (prevFloor == targetFloor)
        {
            
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, floorDoors[targetFloor].destination.position, ref vel, 1/maxSpeed);
            if (Vector3.Distance(transform.position, floorDoors[targetFloor].destination.position) < 0.05f)
            {
                transform.position = floorDoors[targetFloor].destination.position;
                StopAtFloor();
            }
        }

    } 

    void StopAtFloor()
    {
        //Enable New Floor
        floorDoors[targetFloor].OpenFloor();

        //Detatch all survivors from parent room;
        foreach(Survivor surv in room.survivorsInRoom)
        {
            surv.inGameController.gameObject.transform.parent = originalSurvivorTransform;
            surv.inGameController.gameObject.GetComponent<NavMeshAgent>().enabled = true;
        }

        prevFloor = targetFloor;

        if (movedToFloorOnStart)
        {
            SFXManager.s.StopSound(SFXManager.SFXCategory.ElevatorContinuous);
            SFXManager.s.PlaySound(SFXManager.SFXCategory.ElevatorEnd);
        }

        else
        {
            movedToFloorOnStart = true;
        }
    }

    [Button]
    public void MoveToFloor(int floorNum)
    {
        //Close Current Floor
        floorDoors[targetFloor].CloseFloor();


        //Get All Objects In Room, Set as Parent of this.


        BoxCollider col = gameObject.GetComponent<BoxCollider>();
        List<Collider> cols = new List<Collider>(Physics.OverlapBox(col.center, col.size * 0.5f));

        foreach(Survivor surv in room.survivorsInRoom)
        {
            //Run Check If Surivor is Not On Elevator but in room, play error if this happens
            
            if (!cols.Exists(x => x == surv.inGameController.GetComponent<Collider>()))
            {
                Debug.LogError("Survivor In Elevator Room which is not physically in elevator");
            }

            if (originalSurvivorTransform == null)
                originalSurvivorTransform = surv.inGameController.gameObject.transform.parent;

            surv.inGameController.gameObject.transform.parent = transform;
            surv.inGameController.gameObject.GetComponent<NavMeshAgent>().enabled = false;
        }



        //Set new target for the elevator to move to.
        targetFloor = floorNum;
        if (movedToFloorOnStart)
        {
            StartCoroutine(ElevatorStartSoundCoroutine());
        }
    }

    IEnumerator ElevatorStartSoundCoroutine()
    {
        SFXManager.s.PlaySound(SFXManager.SFXCategory.ElevatorStart);
        yield return new WaitWhile(() => SFXManager.s.IsPlaying(SFXManager.SFXCategory.ElevatorStart));
        SFXManager.s.PlaySound(SFXManager.SFXCategory.ElevatorContinuous);
    }
    private void OnTriggerExit(Collider other) {
        SurvivorController controller = other.gameObject.GetComponent<SurvivorController>();
        if (controller != null && controller.data.currentRoom == this)
        {
            Debug.LogError("Survivor Not Leaving Elevator Room Correctly!!!");
        }
        
    }

}
