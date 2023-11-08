using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class VentInteractable : Interactable
{
    [SerializeField, Required]
    private RoomState room;

    private Vent parentVent;

    [SerializeField]
    public Transform exitPoint {get; private set;}

    [SerializeField]
    bool m_SurvivorLocked = true;

    [SerializeField, ShowIf("m_SurvivorLocked")]
    private List<string> m_allowedSurvivorNames;


    public RoomState GetRoom()
    {
        return room;
    }

    public void SetVent(Vent vent)
    {
        parentVent = vent;
    }

    public override void OnInteraction(SurvivorController survivor)
    {
        Debug.Log(survivor.data.m_Name + " going into vent");
        if (m_SurvivorLocked)
        {
            string currentSurvivorName = survivor.data.m_Name;
            if (!m_allowedSurvivorNames.Contains(currentSurvivorName))
            {
                OnInvalidInteraction();
                return;
            }
        }

        parentVent.StartVenting(this, survivor);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
