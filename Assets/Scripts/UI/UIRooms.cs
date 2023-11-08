using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIRooms : MonoBehaviour
{
    public UIDocument doc;
    VisualElement root;

    void Start()
    {
        root = doc.rootVisualElement;
    }

    void Update()
    {
        List<Survivor> _survivors = SurvivorManager.instance.survivors;
        for (int x = 0; x < _survivors.Count; x++)
        {
            TextElement itemElement = root.Q<TextElement>("Character" + x + "RoomDisplay");
            TextElement nameElement = root.Q<TextElement>("Character" + x+ "NameRoom");
            itemElement.text = "";
            nameElement.text = _survivors[x].m_Name;
            
            RoomState room = _survivors[x].currentRoom;
            itemElement.text += "Room: " + room.roomName + "\n";
            itemElement.text += "Power/sec: " + room.powerPerSecond + "\n";
            itemElement.text += "Oxygen: " + room.oxygen + "/" + room.maxOxygen + "\n";
            itemElement.text += "Passive Oxygen: " + room.passiveOxygenRecharge + "\n";
            itemElement.text += "Powered Oxygen: " + room.poweredOxygenRecharge + "\n";
        }
    }
}
