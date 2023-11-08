using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using System;
using UnityEngine.SceneManagement;


public class MedBayInteractable : Interactable
{

    [Serializable]
    public class MedbaySlot
    {
        [SerializeField, ReadOnly]
        private Survivor surv = null;
        [SerializeField, ReadOnly, ] 
        GameObject occupiedGameObject = null;

        [Required]
        public Transform slotPhysicalPosition;

        [ReadOnly]
        public MedBayInteractable parent;

        public bool isOccupied()
        {
            return (occupiedGameObject != null);
        }

        public void RegisterOccupant(Survivor newSurv)
        {
            if (isOccupied())
                return;

            surv = newSurv;
            occupiedGameObject = newSurv.inGameController.gameObject;
            surv.inGameController.CancelMove();
            occupiedGameObject.transform.SetPositionAndRotation(slotPhysicalPosition.position, slotPhysicalPosition.rotation);
        }

        public void ReleaseOccupant()
        {
            if (!isOccupied())
                return;            
            SFXManager.s.PlaySound(SFXManager.SFXCategory.HealthRestore);
            occupiedGameObject.transform.SetPositionAndRotation(parent.exitTransform.position, parent.exitTransform.rotation);
            surv = null;
            surv.inGameController.CancelMove();
            occupiedGameObject = null;
        }

        public void HealSurvivor(float health)
        {
            if (isOccupied())
            { 

                if (SurvivorManager.instance.ChangeSurvivorHealth(surv, health))
                {
                    ReleaseOccupant();
                }
                if (surv.health >= surv.maxHealth)
                {
                    surv.health = surv.maxHealth;
                    ReleaseOccupant();
                }
            }
        }

    }

    public float healthRegenRate = 10;

    private bool doneHealing = true;

    public List<MedbaySlot> currentOccupancy;

    private const string HEAL_STRING = "Healed";
    
    [Required]
    public Transform exitTransform;



    public override void OnInteraction(SurvivorController survivor)
    {
        base.OnInteraction(survivor);
        if (survivor.survivorBeingCarried != null && SurvivorManager.instance.GetSurvivor(survivor.survivorBeingCarried) != null)
        {
            HealCarriedSurvivor(survivor);
            AnalyticsManager.s.AddDataIntValue(SceneManager.GetActiveScene().name + AnalyticsManager.s.MEDBAY_USED_STRING);
        }
        else
        {
            HealActiveSurvivor(survivor.data);
            AnalyticsManager.s.AddDataIntValue(SceneManager.GetActiveScene().name + AnalyticsManager.s.MEDBAY_USED_STRING);
        }
    }


    void Start()
    {
        foreach(var slot in currentOccupancy)
        {
            slot.parent = this;
        }
    }

    void Update()
    {
        HealOccupyingSurvivors();
        
    }

    void HealOccupyingSurvivors()
    {
        foreach(MedbaySlot slot in currentOccupancy)
        {
            slot.HealSurvivor(healthRegenRate * Time.deltaTime);
        }
    }


    void HealActiveSurvivor(Survivor survivor)
    {
        foreach(MedbaySlot slot in currentOccupancy)
        {
            if (!slot.isOccupied())
            {
                slot.RegisterOccupant(survivor);
                return;
            }
        }
        OnFullOccupancy();
    }

    void OnFullOccupancy()
    {
        OnInvalidInteraction();
    }
    
    void HealCarriedSurvivor(SurvivorController survivor)
    {
        Debug.Log("Survivor has stopped moving");
        var carriedSurv = SurvivorManager.instance.GetSurvivor(survivor.survivorBeingCarried);
        
        
        carriedSurv.inGameController.currentAction = SurvivorController.SurvivorAction.standing;
        

        carriedSurv.downedSurvivorInteractable.enabled = false;

        carriedSurv.inGameController.gameObject.transform.parent = null;

        carriedSurv.inGameController.CancelMove();
        

        //Anything else to detach survivor?
        HealActiveSurvivor(carriedSurv);

        survivor.survivorBeingCarried = "";

    

    }

}