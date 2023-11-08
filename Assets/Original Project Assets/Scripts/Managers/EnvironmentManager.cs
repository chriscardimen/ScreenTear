using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// using Sirenix.OdinInspector;
// using DG.Tweening;
using System.Globalization;
using System;

public class EnvironmentManager : MonoBehaviour
{

    private static EnvironmentManager _instance;
    public static EnvironmentManager instance {get {return _instance;}}


    //---ENVIRONMENT VARIABLES---

    //Current Temperature
    public float curTemp;

    //Current Radiation Level
    public float curRads;

    //Current Oxygen Level
    public float curO;


    private float _displayTemp, _displayRads, _displayO;


    //Speed at which text changes values.
    [SerializeField]
    private float _textLerpSpeed = 1f;


    ///Game text for environment

    // [SerializeField, Required, SceneObjectsOnly]
    private TMPro.TextMeshProUGUI _tempTextMesh;

    // [SerializeField, Required, SceneObjectsOnly]
    private TMPro.TextMeshProUGUI _radsTextMesh;

    // [SerializeField, Required, SceneObjectsOnly]
    private TMPro.TextMeshProUGUI _oxyTextMesh;

    // [Button]
    public void ChangeEnvVars(float temp, float rads, float oxy)
    {

        //Store Earlier Value
        _displayTemp = curTemp;
        _displayRads = curRads;
        _displayO = curO;

        //Set New Value
        curTemp = temp;
        curRads = rads;
        curO = oxy;

        //Begin Lerping
        StartCoroutine("LerpEnvVals");


    }

    //Lerps the Environment Values Pretty in the UI
    private IEnumerator LerpEnvVals()
    {
        float startTime = Time.time;

        //Begin Lerp of internal Intervals
        // DOTween.To(()=> _displayTemp, x => _displayTemp = x, curTemp, _textLerpSpeed);
        // DOTween.To(()=> _displayRads, x => _displayRads = x, curRads, _textLerpSpeed);
        // DOTween.To(()=> _displayO, x => _displayO = x, curO, _textLerpSpeed);


        //Display Lerping Value
        while ((Time.time - startTime) <= _textLerpSpeed)
        {
            _tempTextMesh.text = _displayTemp.ToString("F1", CultureInfo.InvariantCulture);
            _radsTextMesh.text = _displayRads.ToString("F", CultureInfo.InvariantCulture);
            _oxyTextMesh.text = _displayO.ToString("F2", CultureInfo.InvariantCulture);
            yield return 0;
        }

        yield return null;

    }

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.Log("Instance already made at - " + _instance.gameObject.name);
            Destroy(this.gameObject);
        } else {
            Debug.Log("Instance established in gameobject - " + gameObject.name);
            _instance = this;
        }


        StartCoroutine("InitialEnvConfig");
    }

    private IEnumerator InitialEnvConfig()
    {
        yield return new WaitWhile(() => MapManager.instance == null);
        Tuple<float, float, float> newstats = MapManager.instance.currentRoom.GetStats();
        ChangeEnvVars(newstats.Item3, newstats.Item2, newstats.Item1);



        yield return null;
    }

}
