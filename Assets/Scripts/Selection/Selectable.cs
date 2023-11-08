using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Selectable : MonoBehaviour
{
    [SerializeField] private UnityEvent MultiClickEvents;
    [SerializeField] private UnityEvent HoverEvents;

    public void OnDoubleClick()
    {
        MultiClickEvents.Invoke();
    }

    public void OnHover()
    {
        HoverEvents.Invoke();
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
