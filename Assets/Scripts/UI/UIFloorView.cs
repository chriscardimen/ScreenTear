using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class UIFloorView : MonoBehaviour
{
    public UIDocument doc;
    private VisualElement root;
    private VisualRoomStatsManager floorManager;
    private Button normalFloor, powerFloor, oxygenFloor;
    public List<ImageColors> images;

    [Serializable]
    public class ImageColors{
        public Texture2D originalImage;
        public Texture2D clickedImage;
    }

    void Start()
    {
        floorManager = VisualRoomStatsManager.instance;
        root = doc.rootVisualElement;
        normalFloor = root.Q<Button>("NormalFloor");
        powerFloor = root.Q<Button>("PowerFloor");
        oxygenFloor = root.Q<Button>("OxygenFloor");

        normalFloor.clicked += SwitchOverlayNormal;
        powerFloor.clicked += SwitchOverlayPower;
        oxygenFloor.clicked += SwitchOverlayOxygen;

    }

    void SwitchOverlayNormal()
    {
        floorManager.SetState(VisualRoomStatsManager.CurrState.Normal);
        ClearImages();
    }

    void SwitchOverlayPower()
    {
        floorManager.SetState(VisualRoomStatsManager.CurrState.Power);
        ClearImages();
        powerFloor.style.backgroundImage = images[0].clickedImage;
    }

    void SwitchOverlayOxygen()
    {
        floorManager.SetState(VisualRoomStatsManager.CurrState.Oxygen);
        ClearImages();
        oxygenFloor.style.backgroundImage = images[1].clickedImage;
    }

    void ClearImages(){
        powerFloor.style.backgroundImage = images[0].originalImage;
        oxygenFloor.style.backgroundImage = images[1].originalImage;
    }

}
