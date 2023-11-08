using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
// using Doozy.Engine.UI;


public class MapDoorOptionGenerator : MonoBehaviour
{
    public OpeningMethod method;

    public GameObject exampleRow;


    // Start is called before the first frame update
    void Start()
    {
    }


    public void UnpackOpeningMethod(OpeningMethod newMethod)
    {
        method = newMethod;
        


        foreach (OpeningMethod.ResourceAllocation alloc in method.neededResources)
        {
            GameObject newRow = Instantiate(exampleRow, transform);

            //Make it so you can interact with newRow
            //newRow.AddComponent<Button>();
            // Navigation nav = new Navigation();
            // nav.mode = Navigation.Mode.None;


            List<Image> icons = new List<Image>(newRow.GetComponentsInChildren<Image>());
            Debug.Log(icons[0].gameObject.name);

            TextMeshProUGUI uiText = newRow.GetComponentInChildren<TextMeshProUGUI>();


            //Check for if puzzle required, if so, delete images and replace text with "Computationally Intensive"


            // If not, set icon 1, 2, the get value of cost and put into text
            // if (!method.isPuzzle)
            // {
            newRow.GetComponentsInChildren<Image>()[1].sprite = ResourceManager.instance
                .characters[alloc.characterIdx].GetComponent<Character>().profilePic;
            newRow.GetComponentsInChildren<Image>()[2].sprite = ResourceManager.instance
                .characters[alloc.characterIdx].GetComponents<ResourceInstance>()[alloc.resourceIdx].resourceIcon;

            //icons[1] = thisRequirement.resourceType.icon
            uiText.text = "-"+alloc.resourceCost.ToString();
            //}
        }
    }

    public void test(GameObject newRow)
    {
        // GameObject.Find("DoorPopup(Clone)").GetComponent<UIPopup>().Hide();
    }
}