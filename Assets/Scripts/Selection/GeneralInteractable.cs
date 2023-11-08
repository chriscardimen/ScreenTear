using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using Sirenix.Serialization;
using System;
public class GeneralInteractable : Interactable
{
    [SerializeField]
    bool m_isInstant = false;

    [SerializeField, ShowIf("@m_isInstant == false")]

    float m_interactionDuration = 10f;

    [SerializeField, Tooltip("Percent Effectiveness if a survivor is working on this interactable while unpowered"), HideIf("m_isInstant"), MinMaxSlider(0f, 1f)]
    private float unpoweredReduction = .7f;


    [SerializeField, ShowIf("m_isInstant")]
    Animation m_instantActionAnim;

    [SerializeField, ShowIf("@m_isInstant == false")]
    Animation m_continuousActionAnim;


    [SerializeField, ReadOnly]
    float progress = 0f;


    [SerializeField, OdinSerialize, DictionaryDrawerSettings(KeyLabel = "Item ID", ValueLabel = "# Required", DisplayMode = DictionaryDisplayOptions.OneLine)]
    public Dictionary<string, int> m_requiredItems = new Dictionary<string, int>();

    public List<string> m_requiredTools = new List<string>();

    public float specialtyMultiplier = 2;

    public bool m_SurvivorLocked = false;




    [SerializeField, ShowIf("m_SurvivorLocked")]
    protected List<string> m_allowedSurvivorNames;

    private Dictionary<string, float> m_interactionMultipliers = new Dictionary<string, float>()
    {
        {SurvivorManager.LOADER_NAME, 1},
        {SurvivorManager.SCOUT_NAME, 1},
        {SurvivorManager.ENGINEER_NAME, 1},
        {SurvivorManager.HACKER_NAME, 1}
    };



    public UnityEvent m_onCompletion;

    void Start()
    {
        SetIndividualInteractionMultipliers();
    }

    void SetIndividualInteractionMultipliers()
    {
        switch (gameObject.name)
        {
            case var doorCase when gameObject.name.Contains("Door"):
                m_interactionMultipliers[SurvivorManager.ENGINEER_NAME] = specialtyMultiplier;
                //Debug.Log($"Engineer Multiplier: {m_interactionMultipliers[SurvivorManager.ENGINEER_NAME]}");
                break;
            case var consoleCase when gameObject.name.Contains("Console"):
                m_interactionMultipliers[SurvivorManager.HACKER_NAME] = specialtyMultiplier;
                //Debug.Log($"Hacker Multiplier: {m_interactionMultipliers[SurvivorManager.HACKER_NAME]}");
                break;
            case var rockCase when gameObject.name.Contains("Rocks"):
                m_interactionMultipliers[SurvivorManager.LOADER_NAME] = specialtyMultiplier;
                //Debug.Log($"Loader Multiplier: {m_interactionMultipliers[SurvivorManager.LOADER_NAME]}");
                break;
        }
    }

    public override void OnInteraction(SurvivorController survivor)
    {
        base.OnInteraction(survivor);
        if (m_SurvivorLocked)
        {
            string currentSurvivorName = survivor.data.m_Name;
            if (!m_allowedSurvivorNames.Contains(currentSurvivorName))
            {
                OnInvalidInteraction();
                return;
            }

        }

        if (m_requiredItems != null && m_requiredItems.Count > 0)
        {
            List<InventoryItem> curItems = survivor.data.inventory.GetInventoryItems();
            List<string> toRemove = new List<string>();
            List<(string, int)> toSubtract = new List<(string, int)>();
            foreach (string item in m_requiredItems.Keys)
            {


                if (curItems.Count <= 0)
                {
                    OnInvalidInteraction();
                    break;
                }

                var check = curItems.FindIndex(x => x.checkID == item);
                int num = curItems[check].amount;
                if (check != -1 && !toRemove.Contains(item))
                {
                    // m_requiredItems[item] -= 1;
                    toSubtract.Add((item, num));

                    if (m_requiredItems[item] <= 0)
                        toRemove.Add(item);

                    if (survivor.data.inventory.GetInventoryItems()[check].isConsumed)
                        survivor.data.inventory.RemoveItem(check);
                }
                else
                {
                    Debug.Log("Survivor named " + survivor.data.m_Name + " doesn't have item of ID " + item, this);
                    OnInvalidInteraction();
                    //Doesn't have all items, ending.
                    return;
                }
            }


            foreach ((string item, int num) in toSubtract)
            {
                m_requiredItems[item] -= num;
                if (m_requiredItems[item] <= 0)
                {
                    toRemove.Add(item);
                }
            }
            foreach (string item in toRemove)
            {
                m_requiredItems.Remove(item);
            }

            foreach (string item in m_requiredTools)
            {
                if (!curItems.Exists(x => x.checkID == item))
                {

                    Debug.Log("Survivor named " + survivor.data.m_Name + " doesn't have tool of ID " + item, this);
                    //Required tool not had, returning
                    OnInvalidInteraction();
                    return;
                }
            }

        }

        //if instant animation...
        if (m_isInstant)
        {
            if (m_instantActionAnim != null)
            {
                //code for instant animation (card swipe, item pickup, etc.)
            }
            if (m_DebugPrompt)
            {
                Debug.Log(endInteractionDebug, this);
            }
            m_onCompletion.Invoke();
            PlaySoundEffectByName();
        }
        //if a continuous animation...
        else
        {

            if (m_continuousActionAnim != null)
            {
                //code for continuous animation
            }
            //If there's no more items needed
            if (m_requiredItems.Count == 0)
            {
                foreach (Survivor surv in SurvivorManager.instance.survivors)
                {
                    if (!surv.Equals(survivor) && surv.interactingWith != null && surv.interactingWith.Equals(GetComponent<GeneralInteractable>()))
                    {
                        return;
                    }
                }
                survivor.currentAction = SurvivorController.SurvivorAction.interacting;
                survivor.data.interactingWith = GetComponent<GeneralInteractable>();
                StartCoroutine("ContinuousInteraction", survivor);
            }

        }
    }

    IEnumerator ContinuousInteraction(SurvivorController controller)
    {
        controller.data.progressBar.gameObject.transform.parent.GetComponent<ProgressIndicator>().progressCanvas.enabled = true;
        while ((controller.currentAction == SurvivorController.SurvivorAction.interacting) && (controller.data.currentState != Survivor.SurvivorState.downed))
        {
            Debug.Log(
                $"Interaction Multiplier for {controller.data.m_Name}: {m_interactionMultipliers[controller.data.m_Name]}");

            float penaltyval = controller.data.inPoweredRoom() ? 1f : unpoweredReduction;
            progress += Time.deltaTime * m_interactionMultipliers[controller.data.m_Name] * penaltyval;
            controller.data.progressBar.value = progress / m_interactionDuration;
            if (progress >= m_interactionDuration)
            {
                //End the interaction
                PlaySoundEffectByName();
                controller.data.interactingWith = null;
                controller.data.progressBar.value = 0;
                controller.data.progressBar.gameObject.transform.parent.GetComponent<ProgressIndicator>().progressCanvas.enabled = false;
                if (m_DebugPrompt)
                {
                    Debug.Log(endInteractionDebug, this);
                }
                m_onCompletion.Invoke();
                yield break;
            }
            if (controller.isMoving())
                yield break;

            yield return new WaitForEndOfFrame();

        }
    }


    private void PlaySoundEffectByName()
    {
        switch (gameObject.name)
        {
            case var doorCase when gameObject.name.Contains("Door"):
                SFXManager.s.PlaySound(SFXManager.SFXCategory.DoorOpen);
                break;
            case var rockCase when gameObject.name.Contains("Rocks"):
                SFXManager.s.PlaySound(SFXManager.SFXCategory.MovingDebris);
                break;
        }
    }




}
