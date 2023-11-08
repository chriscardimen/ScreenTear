using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class VisualRoomStats : MonoBehaviour
{
    public RoomState room;
    public Material currMaterial;
    public Color normalColor, warningColor;
    public float high, low, currPos, mostAmountToMove;

    float fadeDur = 0.25f;

    float updateTimer = 0f;

    private bool wasPowered = false;

    private VisualRoomStatsManager.CurrState prevState = VisualRoomStatsManager.CurrState.Normal;


    private void Start()
    {
        AssignHighLow();
        this.transform.localScale = new Vector3(transform.parent.GetComponent<BoxCollider>().size.x - 0.1f, 0.03736f, transform.parent.GetComponent<BoxCollider>().size.z - 0.1f);
        currMaterial = GetComponent<MeshRenderer>().material;
        room = transform.parent.GetComponent<RoomState>();
        mostAmountToMove = high - low;
        this.transform.localPosition = new Vector3(this.transform.localPosition.x, high, this.transform.localPosition.z);
        MoveOxygenFilling();
        MovePowerMeter(room.GetPower());
    }

    private void AssignHighLow()
    {
        List<VisualRoomStatsManager.HeightPairing> heights = VisualRoomStatsManager.instance.roomHeights;
        int curr_scene = -1;
        for (int y = 0; y < SceneManager.sceneCount; y++)
        {
            if (!SceneManager.GetSceneAt(y).name.Equals("User Interface"))
            {
                curr_scene = (SceneManager.GetSceneAt(y).buildIndex - 1) / 2;
            }
        }

        high = heights[curr_scene].highHeight;
        low = heights[curr_scene].lowHeight;
    }


    public void UpdateRoomStat()
    {
        if (VisualRoomStatsManager.instance.currState != prevState)
        {
            AssignColors(VisualRoomStatsManager.instance.currState);
            if (VisualRoomStatsManager.instance.currState == VisualRoomStatsManager.CurrState.Normal)
            {
                ChangeToNormal();
            }
            else if (VisualRoomStatsManager.instance.currState == VisualRoomStatsManager.CurrState.Oxygen)
            {
                if (currPos >= low)
                {
                    ChangeToOxygen();
                }
            }
            else if (VisualRoomStatsManager.instance.currState == VisualRoomStatsManager.CurrState.Power)
            {
                ChangeToPower();
            }
        }
        prevState = VisualRoomStatsManager.instance.currState;


    }

    private void Update()
    {


        if (prevState == VisualRoomStatsManager.CurrState.Oxygen && currPos >= low)
        {

            MoveOxygenFilling();
        }
    }
    public void UpdatePowerMeter()
    {
        MovePowerMeter(room.GetPower());
    }

    private void ChangeToPower()
    {
        this.transform.DOMoveY(low, 0f);


        if (room.GetPower())
        {
            this.transform.DOMoveY(currPos, fadeDur);
            currPos = high;
            ChangeCurrentColor(normalColor);
        }
        else
        {
            currPos = low;
            ChangeCurrentColor(warningColor);
        }
    }

    private void ChangeToNormal()
    {
        ChangeCurrentColor(normalColor);
    }

    private void ChangeToOxygen()
    {
        float oxygenPercent = room.oxygen / room.maxOxygen;
        float percentToMove = 1 - oxygenPercent;
        currPos = high - (percentToMove * mostAmountToMove);
        this.transform.DOLocalMoveY(low, 0f);
        this.transform.DOLocalMoveY(currPos, fadeDur);
        if (currPos < (low + mostAmountToMove / 2))
        {
            ForceChangeCurrentColor(warningColor);
        }
        else
        {
            ForceChangeCurrentColor(normalColor);
        }
    }

    public void MovePowerMeter(bool isPowered)
    {
        if (wasPowered != isPowered)
        {

            if (isPowered)
            {
                currPos = high;
                this.transform.DOLocalMoveY(currPos, fadeDur);
                ChangeCurrentColor(normalColor);
            }
            else
            {
                currPos = low;
                this.transform.DOLocalMoveY(currPos, fadeDur);
                ChangeCurrentColor(warningColor);
            }
        }

        wasPowered = isPowered;
    }

    public void MoveOxygenFilling()
    {
        if (!DOTween.IsTweening(transform))
        {
            float oxygenPercent = room.oxygen / room.maxOxygen;
            float percentToMove = 1 - oxygenPercent;
            currPos = high - (percentToMove * mostAmountToMove);
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, currPos, this.transform.localPosition.z);
            CheckColor();
        }
    }

    public void CheckColor()
    {
        if (currPos < (low + mostAmountToMove / 2))
        {
            ForceChangeCurrentColor(warningColor);
        }
        else
        {
            ForceChangeCurrentColor(normalColor);
        }
    }

    public void ChangeCurrentColor(Color newColor)
    {

        //currMaterial.color = new Color(newColor.r, newColor.g, newColor.b, newColor.a);
        currMaterial.DOBlendableColor(newColor, fadeDur);
    }

    public void ForceChangeCurrentColor(Color newColor)
    {

        currMaterial.color = new Color(newColor.r, newColor.g, newColor.b, newColor.a);
    }

    void AssignColors(VisualRoomStatsManager.CurrState state)
    {
        List<VisualRoomStatsManager.MaterialPairing> mats = VisualRoomStatsManager.instance.materialList;
        VisualRoomStatsManager.MaterialPairing pair = new VisualRoomStatsManager.MaterialPairing();
        foreach (VisualRoomStatsManager.MaterialPairing pairing in mats)
        {
            if (pairing.type.Equals(state))
            {
                pair = pairing;
            }
        }
        normalColor = pair.safeColor;
        warningColor = pair.warningColor;
    }
}
