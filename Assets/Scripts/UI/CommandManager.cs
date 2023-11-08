using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

public class CommandManager : MonoBehaviour
{
    private static CommandManager _instance;
    public static CommandManager instance { get { return _instance; } }

    [Header("Opening dialogue")]
    [SerializeField]
    private List<DialogueInstance> _dialougeQueue;
    private DialogueInstance _activeInstance;
    [Header("Properties")]
    [SerializeField]
    private float _defaultCPS = 10;
    public UIDocument doc;
    public string scrollViewName;
    private ScrollView _scrollView;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        VisualElement root = doc.rootVisualElement;
        _scrollView = root.Q<ScrollView>(scrollViewName);
    }

    public void AddToQueue(DialogueInstance newDialogue)
    {
        _dialougeQueue.Add(newDialogue);
    }

    void Update()
    {
        if ((_activeInstance == null) && (_dialougeQueue.Count > 0))
        {
            _activeInstance = _dialougeQueue[0];
            _dialougeQueue.RemoveAt(0);
            StartCoroutine("HandleDialogueInstance");
        }
    }

    [Header("CSS Classes")]
    public string instanceContainerClass = "DialogueInstanceContainer";
    public string dialogueClass = "DialogueText";


    IEnumerator HandleDialogueInstance()
    {
        yield return new WaitForSecondsRealtime(_activeInstance.initialPause);

        int count = 0;

        foreach (SingleVoiceLine line in _activeInstance.VoiceLines)
        {
            VisualElement dialogueInstanceContainer = new VisualElement();
            TextElement dialogueText = new TextElement();

            dialogueInstanceContainer.AddToClassList(instanceContainerClass);
            dialogueText.AddToClassList(dialogueClass);

            dialogueInstanceContainer.Add(dialogueText);
            _scrollView.Add(dialogueInstanceContainer);

            float delay = 0;
            if (_defaultCPS != 0)
            {
                delay = 1f / _defaultCPS;
            }


            count++;

            foreach (char c in line.lineOfDialogue)
            {
                dialogueText.text += c;
                yield return new WaitForSecondsRealtime(delay);
            }
            yield return new WaitForSecondsRealtime(line.pause);
        }
        _activeInstance = null;
        yield return null;
    }

}
