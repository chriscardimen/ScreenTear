using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;


public class Interactable : SerializedMonoBehaviour
{
    public string interactableName = "";

    public float interactDistance = 0.6f;

    [Tooltip("Optional interaction location, if not the object's center.")]
    public Transform seperateLocation;

    [HideInInspector] public const string INTERACT_STRING = "interact";


    [SerializeField, Tooltip("Activate Optional Debug Statements")]
    public bool m_DebugPrompt = false;

    [SerializeField, ShowIf("m_DebugPrompt"), Tooltip("Debug Log that triggers when the interaction starts")]
    protected string startInteractionDebug;

    [SerializeField, ShowIf("m_DebugPrompt"), Tooltip("Debug Log that triggers when the interaction ends")]
    protected string endInteractionDebug;


    public virtual void OnInteraction(SurvivorController survivor)
    {
        // Survivor is passed in to make modifications to inventory/health/etc.
        if (m_DebugPrompt)
        {
            Debug.Log("startInteractionDebug", this);
        }
        AnalyticsManager.s.AddDataIntValue(
            interactableName + AnalyticsManager.s.INTERACTED_STRING);
    }

    public Vector3 GoalLocation()
    {
        if (seperateLocation != null)
        {
            return (seperateLocation.position);
        }
        return gameObject.transform.position;
    }

    void Start()
    {
        
    }


    void OnDrawGizmos()
    {
        if (seperateLocation != null)
        {
            Gizmos.DrawWireSphere(seperateLocation.position, interactDistance);
        }
        else
        {
            Gizmos.DrawWireSphere(gameObject.transform.position, interactDistance);
        }
    }

    public void OnInvalidInteraction()
    {
        SFXManager.s.PlaySound(SFXManager.SFXCategory.InvalidInteraction);
    }

}
