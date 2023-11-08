using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Sirenix.OdinInspector;

public class VisualInventory : MonoBehaviour
{
    //===SINGLETON CREATION===
    private static VisualInventory _instance;
    public static VisualInventory instance { get { return _instance; } }

    public void Awake()
    {
        if (_instance != null && _instance != this)
        {
            //Debug.Log("Instance already made at - " + _instance.gameObject.name);
            Destroy(this.gameObject);
        }
        else
        {
            //Debug.Log("Instance established in gameobject - " + gameObject.name);
            _instance = this;
            // Debug.Log(_instance);
            // Debug.Log(instance);
        }
    }

    public UIDocument doc;
    private VisualElement root;
    public List<Survivor> _survivors;
    public InventoryItem testItem;

    void Start()
    {
        root = doc.rootVisualElement;
        _survivors = SurvivorManager.instance.survivors;

        foreach (Survivor surv in _survivors)
        {
            SurvivorInventory inv = surv.inventory;
            foreach (InventoryItem item in inv.GetInventoryItems())
            {
                // Debug.Log("Visual inventory! " + item.itemName);
            }

        }


    }

    public void RefreshInventory()
    {
        for (int x = 0; x < _survivors.Count; x++)
        {
            VisualElement itemOne = root.Q<VisualElement>("Character" + x + "Inv1");
            VisualElement itemTwo = root.Q<VisualElement>("Character" + x + "Inv2");

            SurvivorInventory inv = _survivors[x].inventory;
            List<InventoryItem> items = inv.GetInventoryItems();

            int itemCount = 0;
            foreach (InventoryItem itm in items){
                itemCount += itm.amount;
            }

            switch (itemCount)
            {
                case 0:
                    EmptyItem(itemOne);
                    EmptyItem(itemTwo);
                    break;
                case 1:
                    HasItem(itemOne, items[0].itemImage);
                    EmptyItem(itemTwo);
                    break;
                case 2:
                    if (items[0].amount == 2)
                    {
                        HasItem(itemOne, items[0].itemImage);
                        HasItem(itemTwo, items[0].itemImage);
                    }
                    else
                    {
                        HasItem(itemOne, items[0].itemImage);
                        HasItem(itemTwo, items[1].itemImage);
                    }

                    break;
                default:
                    EmptyItem(itemOne);
                    EmptyItem(itemTwo);
                    break;
            }

        }
    }

    void EmptyItem(VisualElement element)
    {
        element.AddToClassList("InventoryEmpty");
        element.RemoveFromClassList("InventoryItem");
        element.style.backgroundImage = null;
    }

    void HasItem(VisualElement element, Texture2D elementItem)
    {
        element.AddToClassList("InventoryItem");
        element.RemoveFromClassList("InventoryEmpty");
        element.style.backgroundImage = elementItem;

    }

    void Update()
    {
        RefreshInventory();
    }

    [Button("Spawn In Inventory")]
    private void TestSpawn()
    {
        SurvivorManager.instance.survivors[0].inventory.AddToInventory(testItem);
        // SurvivorManager.instance.survivors[1].inventory.AddToInventory(testItem);
    }
}
