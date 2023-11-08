using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UIElements;

public class SelectionManager : MonoBehaviour
{

    //===SINGLETON CREATION===
    private static SelectionManager _instance;
    public static SelectionManager instance { get { return _instance; } }
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
        }
    }


    //====SELECTION MANAGEMENT VARS====
    private Dictionary<int, SurvivorController> selectedSurvivors;

    //====STRING CONSTANTS====
    [HideInInspector] public const string UI_CHILD_NAME_STRING = "ProgressState";

    //====INPUT VARS====
    private Vector2 startMousePos;
    private Vector2 mousePos;

    private bool isHoldingBox = false;

    private Vector3 d_hitPoint;

    private bool additiveToggle = false;
    private bool subtractiveToggle = false;

    private List<GameObject> currentlyHighlightedObject = new List<GameObject>();

    private static int SURVIVOR_CHILD_HIGHLIGHT_INDEX = 1;

    [SerializeField, Required]
    public Camera selectCam;


    [SerializeField]
    private LayerMask leftClickMask;

    [SerializeField]
    private LayerMask rightClickMask;

    [SerializeField]
    private LayerMask hoverMask;

    [SerializeField]
    private bool debugHighLights = false;



    [Header("UI Settings")]
    

    [SerializeField, Required]
    private RectTransform boxCanvasObject;
    [SerializeField]
    private Texture2D defaultCursorTexture;
    [SerializeField]
    private Texture2D selectCursorTexture;

    public CursorMode cursorMode = CursorMode.Auto;

    [SerializeField, Tooltip("In-Game UI Canvas, should be a child of this camera.")]
    private Canvas primaryCanvas;

    [SerializeField]
    private GameObject boxSelectEffect;

    [SerializeField]
    private GameObject goToEffect;


    private void Start()
    {
        selectedSurvivors = new Dictionary<int, SurvivorController>();

        if (boxCanvasObject == null)
        {
            boxCanvasObject = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        }

        UnityEngine.Cursor.SetCursor(defaultCursorTexture, Vector2.zero, cursorMode);
    }

    void Update()
    {
        HandleHold();

        HandleCursor();

    }

    void FixedUpdate()
    {
        if (isHoldingBox)
        {
            HandleBoxSelect();
        }
        else
        {
            HandleSingleHover();
        }
    }

    private void HandleSingleHover()
    {
        RaycastHit hit;
        if (Physics.Raycast(selectCam.ScreenPointToRay(mousePos), out hit, Mathf.Infinity, leftClickMask, QueryTriggerInteraction.Ignore))
        {
            GameObject highlightedObject = hit.collider.gameObject;
            if (!currentlyHighlightedObject.Contains(highlightedObject))
            {

                UnhighlightObjects();
                currentlyHighlightedObject.Add(highlightedObject);
                HighlightObject(highlightedObject);

                UnityEngine.Cursor.SetCursor(selectCursorTexture, Vector2.zero, cursorMode);
            }
        }
        else if (currentlyHighlightedObject.Count != 0)
        {
            UnhighlightObjects();
        }
    }

    private void HandleBoxSelect()
    {

        Vector3 point1 = selectCam.ScreenToWorldPoint(new Vector3((mousePos.x + startMousePos.x) / 2f, (mousePos.y + startMousePos.y) / 2f, 500f));
        float sizeX = selectCam.orthographicSize * selectCam.aspect * Mathf.Abs(startMousePos.x - mousePos.x) / selectCam.pixelWidth;
        float sizeY = selectCam.orthographicSize * Mathf.Abs(startMousePos.y - mousePos.y) / selectCam.pixelHeight;

        RaycastHit hit2;
        if (Physics.Raycast(selectCam.ScreenPointToRay(mousePos), out hit2, Mathf.Infinity, leftClickMask, QueryTriggerInteraction.Ignore))
        {
            GameObject highlightedObject = hit2.collider.gameObject;
            if (!currentlyHighlightedObject.Contains(highlightedObject))
            {
                UnityEngine.Cursor.SetCursor(selectCursorTexture, Vector2.zero, cursorMode);
            }
        }
        else
        {
            UnityEngine.Cursor.SetCursor(defaultCursorTexture, Vector2.zero, cursorMode);
        }

        foreach (RaycastHit hit in Physics.BoxCastAll(point1, new Vector3(sizeX, sizeY, 500f), transform.forward, transform.rotation, Mathf.Infinity, hoverMask, QueryTriggerInteraction.Ignore))
        {
            GameObject highlightedObject = hit.collider.gameObject;
            if (!currentlyHighlightedObject.Contains(highlightedObject))
            {

                currentlyHighlightedObject.Add(highlightedObject);
                HighlightObject(highlightedObject);
            }
        }

    }

    public void UnhighlightObjects()
    {
        if (currentlyHighlightedObject != null)
        {
            if (debugHighLights)
            {
                //Debug.Log($"Unhighlighting {currentlyHighlightedObject}");
            }


            foreach (GameObject obj in currentlyHighlightedObject)
            {
                if (obj == null)
                {
                    currentlyHighlightedObject.RemoveAll(item => item == null);
                    continue;
                }
                if (obj.GetComponent<SurvivorController>() != null &&
                    !selectedSurvivors.ContainsValue(obj.GetComponent<SurvivorController>()))
                {
                    // TODO: Upon selection undoing, you'll need to unhighlight it (unless it's hovered over)
                    // IF THE SURVIVOR PREFAB EVER CHANGES HOW MANY CHILDREN IT HAS
                    // DOUBLE CHECK THAT THIS INDEX IS RIGHT
                    obj.transform.GetChild(SURVIVOR_CHILD_HIGHLIGHT_INDEX).gameObject.layer = LayerMask.NameToLayer("Survivors");
                    UIManager.instance.gameDoc.rootVisualElement.Q<TextElement>("HoverText").text = "//";
                }
                else if (obj.GetComponent<Interactable>() != null && obj.GetComponent<Interactable>().enabled)
                {
                    obj.layer = LayerMask.NameToLayer("Interactables");
                    UIManager.instance.gameDoc.rootVisualElement.Q<TextElement>("HoverText").text = "//";
                }
                SetTextDescriptionActive(obj, false);
            }


            currentlyHighlightedObject = new List<GameObject>();
        }

        UnityEngine.Cursor.SetCursor(defaultCursorTexture, Vector2.zero, cursorMode);
    }

    public void HighlightObject(GameObject obj)
    {
        if (currentlyHighlightedObject.Count > 0)
        {

            if (obj.GetComponent<SurvivorController>() != null)
            {
                // IF THE SURVIVOR PREFAB EVER CHANGES HOW MANY CHILDREN IT HAS
                // DOUBLE CHECK THAT THIS INDEX IS RIGHT
                obj.transform.GetChild(SURVIVOR_CHILD_HIGHLIGHT_INDEX).gameObject.layer = LayerMask.NameToLayer("Highlighted");
                UIManager.instance.gameDoc.rootVisualElement.Q<TextElement>("HoverText").text = "// " + obj.GetComponent<SurvivorController>().data.m_Name;
            }
            else if (obj.GetComponent<Interactable>() != null)
            {
                obj.layer = LayerMask.NameToLayer("Highlighted");
                UIManager.instance.gameDoc.rootVisualElement.Q<TextElement>("HoverText").text = "// " + obj.GetComponent<Interactable>().interactableName;
            }
            SetTextDescriptionActive(obj, true);
        }
    }

    void SetTextDescriptionActive(GameObject obj, bool isActive)
    {
        // TextDescription description = obj.transform.Find("InfoText").gameObject.GetComponent<TextDescription>();

        // if (description == null && debugHighLights)
        // {
        //     Debug.LogError($"GameObject '{gameObject.name}' does not have an InfoText prefab as a child");
        // }

        // //description.SetTextActive(isActive);
    }

    public Vector2 GetMousePos()
    {
        return mousePos;
    }

    public void HandleSelection(GameObject go, Selectable selectable, bool box = false)
    {
        Debug.Log("Hit Selectable!");

        SurvivorController curSurv = go.GetComponent<SurvivorController>();


        if (curSurv == null || !box)
            DeleteAllSelected();
        if (curSurv != null)
        {
            //Cannot Select Survivor if Downed
            if (curSurv.data.currentState == Survivor.SurvivorState.downed)
                return;
            if (subtractiveToggle)
                DeleteSelected(go.GetInstanceID());
            else
                AddSelected(go, curSurv);
        }
    }

    private void AddSelected(GameObject go, SurvivorController surv)
    {
        AnalyticsManager.s.AddDataIntValue(
            surv.data.m_Name + AnalyticsManager.s.SELECTED_STRING);
        int id = go.GetInstanceID();
        if (!(selectedSurvivors.ContainsKey(id)))
        {
            // Debug.Log("ID: " + id);
            selectedSurvivors.Add(id, surv);
            //perform functions on Selection Interface
        }

        if (surv.plumbob != null)
        {
            surv.plumbob.SetActive(true);
        }
    }

    public void DeleteSelected(int id)
    {
        selectedSurvivors[id].transform.GetChild(SURVIVOR_CHILD_HIGHLIGHT_INDEX).gameObject.layer =
            LayerMask.NameToLayer("Survivors");
        if (debugHighLights)
        {
            Debug.Log(
            $"{selectedSurvivors[id]} layer: {selectedSurvivors[id].transform.GetChild(SURVIVOR_CHILD_HIGHLIGHT_INDEX).gameObject.layer}");
        }

        selectedSurvivors.Remove(id);
    }

    public void DeleteAllSelected()
    {
        foreach (SurvivorController surv in selectedSurvivors.Values)
        {
            surv.transform.GetChild(SURVIVOR_CHILD_HIGHLIGHT_INDEX).gameObject.layer =
            LayerMask.NameToLayer("Survivors");
            if (debugHighLights)
            {
                //Debug.Log(
                //$"{surv} layer: {surv.transform.GetChild(SURVIVOR_CHILD_HIGHLIGHT_INDEX).gameObject.layer}");
            }

            if (surv.plumbob != null && !surv.isVenting)
            {
                surv.plumbob.SetActive(false);
            }
        }


        selectedSurvivors.Clear();
    }



    void LeftSelectRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(selectCam.ScreenPointToRay(mousePos), out hit, Mathf.Infinity, leftClickMask, QueryTriggerInteraction.Ignore))
        {
            d_hitPoint = hit.point;


            Selectable selectable = null;
            GameObject go = hit.collider.gameObject;
            // Debug.Log(go);

            if (go != null)
            {
                // Debug.Log("Found"  + go.name +  " checking for selectable");
                selectable = go.GetComponent<Selectable>();

            }

            if (selectable != null)
            {
                // Debug.Log("Selectable Found, handling selection...");
                HandleSelection(go, selectable);
            }
            else
            {
                DeleteAllSelected();
            }
            if (primaryCanvas != null && boxSelectEffect != null)
            {
                GameObject newEffect = GameObject.Instantiate(boxSelectEffect, primaryCanvas.gameObject.transform);
                newEffect.GetComponent<BoxSelectCircle>().StartEffect(new Vector2(selectCam.WorldToScreenPoint(hit.collider.transform.position).x, selectCam.WorldToScreenPoint(hit.collider.transform.position).y));
            }
        }
    }


    void RightSelectRaycast()
    {
        RaycastHit hit;
        //TODO: Refit the raycast to include only item, environemnt, and interactable layers
        if (Physics.Raycast(selectCam.ScreenPointToRay(mousePos), out hit, Mathf.Infinity, rightClickMask, QueryTriggerInteraction.Ignore))
        {
            d_hitPoint = hit.point;

            Interactable interactable = null;
            GameObject go = hit.collider.gameObject;

            if (go != null)
            {
                interactable = go.GetComponent<Interactable>();
            }

            if (interactable != null)
            {
                //Debug.Log("Interacting");
                foreach (KeyValuePair<int, SurvivorController> pair in selectedSurvivors)
                {
                    if (pair.Value.enabled)
                    {
                        //Debug.Log("Moving to interactable");
                        pair.Value.MoveToInteractable(interactable);

                        //Add Interaction UI


                    }
                }
            }
            else
            {
                foreach (KeyValuePair<int, SurvivorController> pair in selectedSurvivors)
                {
                    // Debug.Log("Moving " + pair.Value.data.m_Name);
                    if (pair.Value.enabled)
                    {
                        // Debug.Log("Trying to Move Object");
                        pair.Value.MoveTo(hit.point, selectedSurvivors.Count > 1 ? 3f : 0f, pair.Value.data.moveSpeed);
                        // Debug.Log("Pair.Value.isMoving: " + pair.Value.isMoving());

                    }
                }
            }
        }
    }




    [Button, HideInEditorMode]
    private void KillBind()
    {
        KillSelected();
    }

    private void KillSelected()
    {
        Debug.Log(SurvivorManager.instance);
        Debug.Log(selectedSurvivors.First().Value.data);
        SurvivorManager.instance.ChangeSurvivorHealth(
            selectedSurvivors.First().Value.data, -selectedSurvivors.First().Value.data.health);
        Debug.Log(selectedSurvivors.First().Value.data.m_Name + " is Dead");
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        mousePos = context.action.ReadValue<Vector2>();
    }

    public void OnLeftClick(InputAction.CallbackContext context)
    {
        if (context.interaction is TapInteraction && context.performed)
        {
            DeleteAllSelected();
        }

        if (context.interaction is PressInteraction || context.interaction is TapInteraction)
        {
            if (context.started)
            {
                startMousePos = mousePos;
            }

            if (context.performed)
            {
                // Debug.Log("Performing Left Click Tap");

                LeftSelectRaycast();
            }
        }





        // if (context.interaction is HoldInteraction)
        // {
        //     if (context.started)
        //         startMousePos = mousePos;
        //     else if (context.performed)
        //         GenerateSelectionBox();
        //     else if (context.canceled)
        //         SelectFromBox();


        // }

        if (context.interaction is MultiTapInteraction)
        {

            // Debug.Log("Initiating Double Click");
            if (context.performed)
            {

                RaycastHit hit;
                if (Physics.Raycast(selectCam.ScreenPointToRay(mousePos), out hit))
                {
                    Selectable objSelect = hit.collider.gameObject.GetComponent<Selectable>();
                    if (objSelect != null)
                    {
                        objSelect.OnDoubleClick();
                    }

                }

            }


        }

        if (context.interaction is SlowTapInteraction)
        {
            if (context.started)
            {
                DeleteAllSelected();
                startMousePos = mousePos;
                isHoldingBox = true;
                Debug.Log("Started Hold");
            }
            if (context.performed && isHoldingBox)
            {
                SelectBox();
                isHoldingBox = false;
                Debug.Log("Hold Complete!");
            }
            if (context.canceled)
            {
                isHoldingBox = false;
                UnhighlightObjects();
                Debug.Log("Holding Canceled");
            }

        }


    }

    private void SelectBox()
    {
        Vector3 point1 = selectCam.ScreenToWorldPoint(new Vector3((mousePos.x + startMousePos.x) / 2f, (mousePos.y + startMousePos.y) / 2f, 500f));
        float sizeX = selectCam.orthographicSize * selectCam.aspect * Mathf.Abs(startMousePos.x - mousePos.x) / selectCam.pixelWidth;
        float sizeY = selectCam.orthographicSize * Mathf.Abs(startMousePos.y - mousePos.y) / selectCam.pixelHeight;

        foreach (RaycastHit hit in Physics.BoxCastAll(point1, new Vector3(sizeX, sizeY, 500f), transform.forward, transform.rotation, Mathf.Infinity, hoverMask, QueryTriggerInteraction.Ignore))
        {
            Selectable selectable = null;
            GameObject go = hit.collider.gameObject;
            // Debug.Log(go);

            if (go != null)
            {
                // Debug.Log("Found"  + go.name +  " checking for selectable");
                selectable = go.GetComponent<Selectable>();

            }

            if (selectable != null)
            {
                // Debug.Log("Selectable Found, handling selection...");
                HandleSelection(go, selectable, true);
            }

            if (primaryCanvas != null && boxSelectEffect != null)
            {
                GameObject newEffect = GameObject.Instantiate(boxSelectEffect, primaryCanvas.gameObject.transform);
                newEffect.GetComponent<BoxSelectCircle>().StartEffect(new Vector2(selectCam.WorldToScreenPoint(hit.collider.transform.position).x, selectCam.WorldToScreenPoint(hit.collider.transform.position).y));
            }
        }
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            // Debug.Log("Performing Right Click");
            RightSelectRaycast();
        }
    }

    private void HandleCursor()
    {

    }

    private void HandleHold()
    {
        boxCanvasObject.gameObject.SetActive(isHoldingBox);




        boxCanvasObject.sizeDelta = new Vector2(Mathf.Abs(mousePos.x - startMousePos.x), Mathf.Abs(mousePos.y - startMousePos.y));
        boxCanvasObject.position = Vector2.Lerp(mousePos, startMousePos, 0.5f);


        if (isHoldingBox)
        {

        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(d_hitPoint, 0.5f);
        Color oldcolor = Gizmos.color;
        Gizmos.color = Color.yellow;
        if (isHoldingBox)
        {
            Vector3 point1 = selectCam.ScreenToWorldPoint(new Vector3((mousePos.x + startMousePos.x) / 2f, (mousePos.y + startMousePos.y) / 2f, 500f));
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(point1, 0.25f);
            Gizmos.DrawSphere(selectCam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0f)), 0.25f);

            float sizeX = selectCam.orthographicSize * 2f * selectCam.aspect * (mousePos.x - startMousePos.x) / selectCam.pixelWidth;
            float sizeY = selectCam.orthographicSize * 2f * (mousePos.y - startMousePos.y) / selectCam.pixelHeight;


            Gizmos.color = Color.yellow;
            Gizmos.DrawWireMesh(Resources.GetBuiltinResource<Mesh>("Cube.fbx"), point1, transform.rotation, new Vector3(sizeX, sizeY, 1000f));
        }

        Gizmos.color = oldcolor;
    }

    void OnDrawGizmosSelected()
    {

    }

    public List<Survivor> getSelectedSurvivors()
    {
        List<Survivor> survivors = new List<Survivor>();
        if (selectedSurvivors.Count > 0)
        {
            foreach (var x in selectedSurvivors)
            {
                survivors.Add(x.Value.data);
            }
        }
        return survivors;
    }

}
