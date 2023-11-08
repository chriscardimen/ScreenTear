using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Assistant : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public GameObject text;
    public float textSpeed = 0.1f;

    private void Awake()
    {

        //messageText = transform.Find("message").Find("messageText").GetComponent<TextMeshProUGUI>();
    }

    /*public void FastForward()
    {
        string[] messageArray = new string[]
        {
            "You are our last hope.",
            "Blah blah haha funny goofy.",
            "I want to be inside the sun right now.",
        };

        string message = messageArray[Random.Range(0, messageArray.Length)];
        TextWriter.AddWriter_Static(messageText, message, .05f, true);

    }*/

    // Start is called before the first frame update
    private void Start()
    {
        if (text.activeSelf)
        {
            TextWriter.AddWriter_Static(messageText, "Goddess of Safety and Protection", textSpeed, true);
        }
    }

}
