using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

public class SurvivorController : MonoBehaviour
{
    [SerializeField]

    private NavMeshAgent agent;

    [ReadOnly]
    public Survivor data;

    [ReadOnly]
    public string survivorBeingCarried;

    public enum SurvivorAction { standing, moving, interacting, following };

    public SurvivorAction currentAction = SurvivorAction.standing;

    public float idleTime = 0f;

    public GameObject plumbob;

    public bool isVenting;
    
    [SerializeField, ReadOnly]
    public MedBayInteractable.MedbaySlot currentResidingMedbay { private get; set; } = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateSurvivorState();
        CheckIfStuck();
    }

    private void CheckIfStuck()
    {
        if (agent.pathStatus != NavMeshPathStatus.PathComplete)
        {
            Debug.Log("Failed");
            CancelMove();
        }
    }

    public void UpdateSurvivorState()
    {
        if (currentAction == SurvivorAction.moving && !isMoving())
            currentAction = SurvivorAction.standing;
        if (currentAction == SurvivorAction.standing && isMoving())
            currentAction = SurvivorAction.moving;
        if (currentAction == SurvivorAction.standing)
            AnalyticsManager.s.AddDataFloatValue(AnalyticsManager.s.IDLE_TIME_STRING, Time.deltaTime);
    }

    public bool isMoving()
    {
        return agent.velocity.magnitude > 0.01f;
    }

    public void MoveTo(Vector3 point, float stoppingDistance = 0, float speed = 3.5f)
    {
        if (currentResidingMedbay != null)
        {
            currentResidingMedbay.ReleaseOccupant();
        }

        speed = NerfSpeed(speed);

        currentAction = SurvivorAction.moving;
        data.interactingWith = null;
        data.progressBar.gameObject.transform.parent.GetComponent<ProgressIndicator>().progressCanvas.enabled = false;
        agent.speed = speed;
        agent.stoppingDistance = Mathf.Max(stoppingDistance, 0) - 1f;


        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(point, path);
        if (path.status == NavMeshPathStatus.PathPartial)
        {
            //What would you want done if the survivor can't move there? Currently just cancelling their movement
            CancelMove();
            //There should be a UI notification for when a path is invalid
        }
        else
        {
            agent.SetDestination(point);
        }

    }
    public void Follow(Transform tr, float stoppingDistance = 1f, float speed = 3.5f)
    {
        if (currentResidingMedbay != null)
            currentResidingMedbay.ReleaseOccupant();
        currentAction = SurvivorAction.moving;
        agent.speed = speed;
        agent.stoppingDistance = stoppingDistance;
        currentAction = SurvivorAction.following;
        StartCoroutine("FollowTarget", tr);
    }

    IEnumerator FollowTarget(Transform tr)
    {
        while (currentAction == SurvivorAction.following)
        {
            agent.SetDestination(tr.position);
            yield return new WaitForEndOfFrame();
        }
        CancelMove();

        yield return null;
    }



    public void CancelMove()
    {
        agent.isStopped = true;
        agent.ResetPath();
    }

    public void MoveToInteractable(Interactable interactable)
    {
        if (interactable.gameObject != null)
            StartCoroutine(MoveToInteractableCoroutine(interactable));
    }

    IEnumerator MoveToInteractableCoroutine(Interactable interactable)
    {
        if (interactable != null)
        {
            MoveTo(interactable.GoalLocation(), interactable.interactDistance, data.moveSpeed);
        }
        
        while (interactable != null &&
               (isMoving() || (Vector3.Distance(gameObject.transform.position, interactable.GoalLocation())
                               > interactable.interactDistance)))
        {
            yield return null;
        }
        
        if (interactable != null)
        {
            Debug.Log("Distance Between Controller and Target: " + Vector3.Distance(gameObject.transform.position, interactable.GoalLocation()).ToString());
            interactable.OnInteraction(this);
        }
    }

    private float NerfSpeed(float speed)
    {
        float speedPenalty = survivorBeingCarried == SurvivorManager.LOADER_NAME ?
            data.unpoweredMoveSpeedPenalty : 0.5f;
        if (CheckNerfSpeed())
        {
            speed *= speedPenalty;
        }

        return speed;
    }

    private bool CheckNerfSpeed()
    {
        return survivorBeingCarried != null &&
               survivorBeingCarried != SurvivorManager.SCOUT_NAME &&
               data.m_Name != SurvivorManager.LOADER_NAME
               && SurvivorManager.instance.GetSurvivor(survivorBeingCarried) != null;
    }
}
