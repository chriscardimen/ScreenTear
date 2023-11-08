using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using System;
// using Sirenix.OdinInspector;

public class DialogueManager : MonoBehaviour
{


    private static DialogueManager _instance;
    public static DialogueManager instance { get { return _instance; } }


    [SerializeField]
    // [DisableInPlayMode]
    private List<DialogueInstance> _dialougeQueue;


    private DialogueInstance _activeInstance;

    [SerializeField]
    private float _defaultCPS = 10;


    // private TMPro.TextMeshProUGUI _textMesh;
    public UIDocument doc;
    public string scrollViewName;
    public ScrollView _scrollView;
    public TextElement notificationCounter;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            // Debug.Log("Instance already made at - " + _instance.gameObject.name);
            Destroy(this.gameObject);
        }
        else
        {
            // Debug.Log("Instance established in gameobject - " + gameObject.name);
            _instance = this;
        }

        VisualElement root = doc.rootVisualElement;
        _scrollView = root.Q<ScrollView>(scrollViewName);
        notificationCounter = root.Q<TextElement>("NotificationCount");
    }

    public void AddToQueue(DialogueInstance newDialogue)
    {
        _dialougeQueue.Add(newDialogue);
    }

    void Start()
    {
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

    public string instanceContainerClass = "DialogueInstanceContainer";
    public string dialogueClass = "DialogueText";
    public string dialogueImageClass = "DialogueImageContainer";
    public string dialogueCharacterProfile = "DialogueCharacterPic";


    IEnumerator HandleDialogueInstance()
    {
        float cps = _activeInstance.overrideCPS > 0 ? _activeInstance.overrideCPS : _defaultCPS;
        yield return new WaitForSeconds(_activeInstance.initialPause);

        int count = 0;

        foreach (SingleVoiceLine line in _activeInstance.VoiceLines)
        {
            if ((_scrollView.style.display == DisplayStyle.None))
            {
                notificationCounter.visible = true;
                notificationCounter.text = (Int32.Parse(notificationCounter.text) + 1).ToString();
            }
            if (SFXManager.s != null)
            {
                SFXManager.s.PlaySound(SFXManager.SFXCategory.ChatMessage);

            }

            VisualElement dialogueInstanceContainer = new VisualElement();
            TextElement dialogueText = new TextElement();
            VisualElement dialogueImageContainer = new VisualElement();
            VisualElement dialogueChatacterProfile = new VisualElement();


            dialogueInstanceContainer.AddToClassList(instanceContainerClass);
            dialogueText.AddToClassList(dialogueClass);
            dialogueImageContainer.AddToClassList(dialogueImageClass);
            dialogueChatacterProfile.AddToClassList(dialogueCharacterProfile);


            Background picture = new Background();
            picture.sprite = line.characterImage;

            dialogueChatacterProfile.style.backgroundImage = picture;

            dialogueImageContainer.Add(dialogueChatacterProfile);
            dialogueInstanceContainer.Add(dialogueText);
            dialogueInstanceContainer.Add(dialogueImageContainer);
            _scrollView.Add(dialogueInstanceContainer);

            float delay = 1f / (line.overrideCPS > 0 ? line.overrideCPS : cps);

            count++;

            foreach (char c in line.lineOfDialogue)
            {

                dialogueText.text += c;

                yield return new WaitForSeconds(delay);
            }
            yield return new WaitForSeconds(line.pause);
            VisualElement inputContainer = doc.rootVisualElement.Q<VisualElement>("unity-content-container");
            int childCount = inputContainer.childCount;
            if (childCount > 1)
            {
                _scrollView.ScrollTo(inputContainer[childCount - 1]);
            }
        }
        _activeInstance = null;
        yield return null;
    }

}
