using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Vent : MonoBehaviour
{

    public List<Transform> ventPath;

    [SerializeField]
    private VentInteractable startingVent;

    [SerializeField]
    private VentInteractable endingVent;

    [SerializeField]
    private RoomState room;

    [SerializeField]
    private float moveSpeed = 1f;

    [SerializeField] 
    private bool ventOccupied = false;



    // Start is called before the first frame update
    void Start()
    {
        startingVent.SetVent(this);
        endingVent.SetVent(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartVenting(VentInteractable vent, SurvivorController controller)
    {
        StartCoroutine(MoveSurvivor(vent, controller));
    }

    private IEnumerator MoveSurvivor(VentInteractable vent, SurvivorController controller)
    {
        
        Debug.Log("Moving Into Vent");
        ventOccupied = true;
        controller.data.changeCurrentRoom(room);
        controller.gameObject.GetComponent<NavMeshAgent>().enabled = false;
        controller.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        GameObject model = controller.gameObject.transform.GetChild(1).gameObject;
        model.SetActive(false);
        controller.isVenting = true;
        List<Transform> totalpoints = new List<Transform>();
        totalpoints.AddRange(ventPath);
        VentInteractable exitVent = endingVent;
        if (vent == endingVent)
        {
            exitVent = startingVent;
            totalpoints.Reverse();
        }

        totalpoints.Add(exitVent.exitPoint);

        StartCoroutine(VentSoundCoroutine());

        foreach(Transform tr in totalpoints) {
            Vector3 pos = tr.position;
            while (Vector3.Distance(controller.transform.position, pos) > .0001) {
                controller.transform.position = Vector3.MoveTowards(controller.transform.position, pos, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
        
        controller.data.changeCurrentRoom(exitVent.GetRoom());

        controller.gameObject.GetComponent<NavMeshAgent>().enabled = true;
        controller.gameObject.GetComponent<CapsuleCollider>().enabled = true;
        model.SetActive(true);
        if (!SelectionManager.instance.getSelectedSurvivors().Contains(controller.data))
        {
            controller.plumbob.SetActive(false);
        }

        controller.isVenting = false;
        ventOccupied = false;
        yield return null;
    }

    private void OnDrawGizmos() {
        
        // if (UnityEditor.Selection.transforms.Intersect(transform.GetComponentsInChildren<Transform>()).Count<Transform>() == 0) { return; }
        // Gizmos.color = Color.red;
        // Gizmos.DrawWireSphere(startingVent.exitPoint.position, 0.5f);
        // Gizmos.DrawWireSphere(endingVent.exitPoint.position, 0.5f);
        // Gizmos.color = Color.green;
        // if (ventPath.Count == 0)
        //     return;
        // Gizmos.DrawLine(startingVent.exitPoint.position, ventPath[0].position);
        // for (int i = 0; i < ventPath.Count-1; i++)
        // {
        //     Gizmos.DrawWireSphere(ventPath[i].position, 0.25f);
        //     Gizmos.DrawLine(ventPath[i].position, ventPath[i+1].position);
        // }
        // Gizmos.DrawWireSphere(ventPath[ventPath.Count-1].position, 0.25f);
        // Gizmos.DrawLine(ventPath[ventPath.Count-1].position, endingVent.exitPoint.position);

        
    }

    private IEnumerator VentSoundCoroutine()
    {
        while (true)
        {
            SFXManager.s.PlaySound(SFXManager.SFXCategory.VentStep);
            if (!ventOccupied)
            {
                SFXManager.s.StopSound(SFXManager.SFXCategory.VentStep);
                break;
            }
            yield return new WaitForSeconds(0.5f);
        }
        yield return null;
    }
    
}
