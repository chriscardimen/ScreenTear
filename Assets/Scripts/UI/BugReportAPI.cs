using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class BugReportAPI : MonoBehaviour
{
    static public bool DEBUG = true;
    public UIDocument gameDoc;
    public UIDocument bugDoc;
    private DropdownField drop;
    private TextField bugName, bugEmail, bugComment;
    private VisualElement gameRoot, bugRoot, bugContainer;

    PlayerInput input;
    void Start()
    {
        input = SelectionManager.instance.gameObject.GetComponent<PlayerInput>();
        gameRoot = gameDoc.rootVisualElement;
        bugRoot = bugDoc.rootVisualElement;
        bugContainer = bugRoot.Q<VisualElement>("Container");

        Button bugReportToggle = gameRoot.Q<Button>("BugReportToggle");
        Button bugReportCancel = bugRoot.Q<Button>("CancelButton");
        Button bugReportSubmit = bugRoot.Q<Button>("SubmitButton");
        bugReportToggle.clicked += BugScreenOn;
        bugReportCancel.clicked += BugScreenOff;
        bugReportSubmit.clicked += SubmitReport;

        bugName = bugRoot.Q<TextField>("NameInput");
        bugEmail = bugRoot.Q<TextField>("EmailInput");
        bugComment = bugRoot.Q<TextField>("CommentInput");
        drop = bugRoot.Q<DropdownField>("DropdownInput");
    }

    void BugScreenOn()
    {
        bugContainer.visible = true;
        input.actions.Disable();
    }

    void BugScreenOff()
    {
        bugContainer.visible = false;
        input.actions.Enable();
    }

    void SubmitReport()
    {
        // Debug.Log(bugName.text);
        // Debug.Log(bugEmail.text);
        // Debug.Log(bugComment.text);
        // Debug.Log(drop.choices[drop.index]);
        SubmitFeedback();
    }

    void SubmitFeedback()
    {
        WWWForm wForm = new WWWForm();
        wForm.AddField("userName", bugName.text);
        wForm.AddField("userEmail", bugEmail.text);
        wForm.AddField("comment", bugComment.text);
        string reportType = drop.choices[drop.index];

        wForm.AddField("reportType", reportType);
        wForm.AddField("version", Application.version);

        XnBugReporter.POST(wForm, SubmitFeedbackCallback);
        ClearInputs();
        BugScreenOff();
    }

    void ClearInputs(){
        bugComment.value = "";
        drop.index = 0;
    }

    void SubmitFeedbackCallback(bool success, string note)
    {
        if (success)
        {
            Debug.Log("Success!");
            // XnModalPanel.SHOW($"Success!\n\nThank you for submitting your comment.");
        }
        else
        {
            Debug.Log("Error...");
            // XnModalPanel.SHOW($"I'm sorry, but the comment didn't make it to our servers. Please submit it again later.");
        }
    }

}
