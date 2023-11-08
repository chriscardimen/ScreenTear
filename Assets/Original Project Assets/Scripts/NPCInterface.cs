using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
// using Sirenix.OdinInspector;
using System;
// using DG.Tweening;
// using Doozy.Engine.UI;
public class NPCInterface : MonoBehaviour
{
    [Serializable]
    private class ResourceDisplay
    {
        public Image resourceIcon;
        public TextMeshProUGUI amount;
        public TextMeshProUGUI max;

        public TextMeshProUGUI change;

        // [Button]
        private void AutoUpload(GameObject obj)
        {
            resourceIcon = obj.transform.GetChild(0).gameObject.GetComponent<Image>();
            amount = obj.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
            max = obj.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
            change = obj.transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>();

        }


    }

    [SerializeField]
    float _alterationSpeed = 0.5f;
    [SerializeField]
    float _alterationDelay = 0.5f;

    GameObject _relevantChar;


    public TextMeshProUGUI nameField;

    [SerializeField]
    private List<ResourceDisplay> resources;
    
    public void InitializeResources(GameObject character)
    {
        _relevantChar = character;

        Character theChar;
        
        if ( _relevantChar.TryGetComponent(out theChar))
        {
            nameField.text = theChar.npcName;
        }
        else
        {
            Debug.LogError("no Character Component in aquired Object");
            return;
        }


        for(int i = 0; i < resources.Count; i++)
        {
            ResourceInstance activeResouce = _relevantChar.GetComponents<ResourceInstance>()[i];
            
            resources[i].resourceIcon.sprite = activeResouce.resourceIcon;
            resources[i].amount.text = activeResouce.currAmount.ToString();
            resources[i].max.text = activeResouce.maxAmount.ToString();
            resources[i].change.text = "";
        }

    }

    public void ChangeResource(int idx, int change)
    {
        int currAmount = _relevantChar.GetComponents<ResourceInstance>()[idx].currAmount;
        object[] parms = new object[3]{idx, currAmount, change};
        StartCoroutine("StartResourceAnim", parms);
    }

    private IEnumerator StartResourceAnim(object[] parms)
    {

        // yield return new WaitWhile(() => (FindObjectOfType<UIPopup>() != null));

        Debug.Log("Starting Change");

        int idx = (int)parms[0];
        int amount = (int)parms[1];
        int change = (int)parms[2];
        
        ResourceDisplay curDisplay = resources[idx];

        // curDisplay.change.DOFade(1f, _alterationDelay/2f);

        curDisplay.change.color = (amount < 1) ? Color.green : Color.red;

        curDisplay.amount.fontStyle = FontStyles.Bold;

        float startTime = Time.time;

        yield return new WaitForSeconds(_alterationDelay);

        //Begin Lerp of internal Intervals
        // DOTween.To(()=> amount, x => amount = x, amount + change, _alterationSpeed);
        // DOTween.To(()=> change, x => change = x, 0, _alterationSpeed);


        //Display Lerping Value
        while ((Time.time - startTime) <= _alterationSpeed)
        {

            

            curDisplay.amount.text = amount.ToString();
            curDisplay.change.text = ((change > 0) ? "+" : "") + (change != 0 ? change.ToString() : "") ;
            yield return 0;
        }

        yield return new WaitForSeconds(_alterationDelay);


        curDisplay.amount.fontStyle = FontStyles.Normal;

        // curDisplay.change.DOFade(0f, 0f);

        yield return null;
    }

}
