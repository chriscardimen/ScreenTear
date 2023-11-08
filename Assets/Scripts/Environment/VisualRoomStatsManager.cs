using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using System;
using DG.Tweening;
using UnityEngine.UI;

public class VisualRoomStatsManager : MonoBehaviour
{

    [Header("Properties")]
    [ReadOnly]
    public List<VisualRoomStats> allRooms;
    [Header("Heights")]
    public List<HeightPairing> roomHeights;
    [Header("Material state pairings")]
    public List<MaterialPairing> materialList;
    public CurrState currState;

    public enum CurrState
    {
        Normal,
        Oxygen,
        Power
    }

    [Serializable]
    public class MaterialPairing
    {
        public CurrState type;
        public Color safeColor;
        public Color warningColor;
    }

    [Serializable]
    public class HeightPairing{
        public int levelNumber;
        public float highHeight;
        public float lowHeight;
    }

    private static VisualRoomStatsManager _instance;
    public static VisualRoomStatsManager instance { get { return _instance; } }

    public void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        GameObject[] rooms = GameObject.FindGameObjectsWithTag("RoomFilling");
        foreach (GameObject room in rooms)
        {
            allRooms.Add(room.GetComponent<VisualRoomStats>());
        }
        SetState(VisualRoomStatsManager.CurrState.Normal);
    }

    public void SetState(CurrState state)
    {
        var OldState = currState;
        currState = state;

        foreach(VisualRoomStats stat in FindObjectsOfType<VisualRoomStats>())
        {
            stat.UpdateRoomStat();
        }

        //Enable All Power Buttons!
        
        foreach(PowerButton button in FindObjectsOfType<PowerButton>())
        {
            button.gameObject.GetComponent<Canvas>().enabled = (state == CurrState.Power);
            if (OldState != CurrState.Power && state == CurrState.Power)
            {
                var image = button.gameObject.GetComponentInChildren<Image>();
                var tempColor = image.color;
                tempColor.a = 0f;
                image.color = tempColor;
                image.DOFade(1f, 0.25f);
            }
        }

        
    }
}
