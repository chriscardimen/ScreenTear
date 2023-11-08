using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using Cinemachine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class IsometricCameraController : MonoBehaviour
{
    [Header("Required Game Objects")]
    [FormerlySerializedAs("center")][SerializeField] private Transform pivot;
    [SerializeField] private Camera camera;

    [Header("Camera Properties")]
    [SerializeField] float totalRotationAmount = 45f;
    [SerializeField] float accelerationTime = 0.25f;
    [SerializeField] float rotationTime = 0.05f;
    [SerializeField] float snapTime = 0.05f;
    [SerializeField] float scrollsens = 0.25f;
    [SerializeField] float maxScroll = 50;
    [SerializeField] float minScroll = 10;
    [Tooltip("How deep the pivot pivot should be relative to the camera object.")]
    [SerializeField] float centerPivotDepth = 1000f;

    [Header("InputActions")]
    public InputAction moveCamera;
    public InputAction zoomCamera;
    public InputAction rotateCamera;
    public InputAction snapToLoader;
    public InputAction snapToHacker;
    public InputAction snapToScout;
    public InputAction snapToEngineer;

    private Vector3 currentVelocity = Vector3.zero;
    private bool isRotating = false;
    private bool isSnapping = false;
    private Vector3 targetEulerAngles;

    private void Start()
    {
        EnableInputActions();
        CheckRequiredGameObjects();
    }

    void CheckRequiredGameObjects()
    {
        if (pivot == null)
        {
            Debug.LogError("Main camera does not have a transform to pivot around");
        }

        if (gameObject.GetComponent<Camera>() == null)
        {
            Debug.LogError("Main Camera doesn't have an attached Camera object");
        }

        camera = gameObject.GetComponent<Camera>();
        pivot.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z) +
                                   transform.forward * centerPivotDepth;
        transform.parent = pivot;
        pivot.transform.position =
            SurvivorManager.instance.GetSurvivor(SurvivorManager.LOADER_NAME)
                .inGameController.transform.position;
    }

    void EnableInputActions()
    {
        moveCamera.Enable();
        zoomCamera.Enable();
        rotateCamera.Enable();
        snapToEngineer.Enable();
        snapToHacker.Enable();
        snapToScout.Enable();
        snapToLoader.Enable();
    }

    void Update()
    {
        ProcessCameraControls();
    }

    void ProcessCameraControls()
    {
        if (!UIManager.instance.terminalPopup.rootVisualElement.Q<VisualElement>("Container").visible)
        {
            ProcessMoveCamera();
            ProcessZoomCamera();
            ProcessRotateCameraRequest();
            ProcessSnapToSelectedCharacter();
        }
    }

    void ProcessMoveCamera()
    {
        if (!isRotating && !isSnapping)
        {
            Vector2 inputDirection = moveCamera.ReadValue<Vector2>();
            Vector3 currentMoveDirection = default;
            float accelerationMultiplier = 0;
            if (Time.deltaTime != 0) { accelerationMultiplier = accelerationTime / Time.deltaTime; }
            currentMoveDirection += inputDirection.x * transform.right * accelerationMultiplier;
            currentMoveDirection += inputDirection.y * transform.up * accelerationMultiplier;

            pivot.transform.position = Vector3.SmoothDamp(pivot.transform.position, pivot.transform.position + currentMoveDirection,
                ref currentVelocity, accelerationTime);
        }
    }

    void ProcessZoomCamera()
    {
        if (SelectionManager.instance.GetMousePos().x > Screen.width * 0.30)
        {
            float scrollval = zoomCamera.ReadValue<float>();
            //transform.position += (scrollsens * scrollval * transform.forward);
            float orthoSize = GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize;
            orthoSize += -scrollval * scrollsens;
            if (orthoSize > maxScroll)
            {
                orthoSize = maxScroll;
            }
            else if (orthoSize < minScroll)
            {
                orthoSize = minScroll;
            }

            GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = orthoSize;
        }
    }

    void ProcessRotateCameraRequest()
    {
        // I'm storing this in a separate variable first in case
        // The button is pushed while the rotation direction shouldn't be
        // updated.

        float rotationDirection = rotateCamera.ReadValue<float>();

        if (rotationDirection != 0 && !isRotating)
        {
            isRotating = true;
            StartCoroutine(RotationCoroutine(rotationDirection));
        }
    }

    IEnumerator RotationCoroutine(float rotationDirection)
    {
        float startTime = Time.time;
        float startEulerY = pivot.transform.eulerAngles.y;
        Quaternion startRotation = pivot.transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0f, (int) (rotationDirection * totalRotationAmount), 0f);
        while (Time.time < startTime + rotationTime)
        {
            float progress = (Time.time - startTime) / rotationTime;
            pivot.transform.rotation = Quaternion.Slerp(startRotation, endRotation, progress);
            yield return null;
        }

        if (pivot.transform.eulerAngles.y != startEulerY + rotationDirection * totalRotationAmount)
        {
            Debug.Log($"Euler Y Before: {pivot.transform.eulerAngles.y}");
            pivot.transform.rotation = Quaternion.Euler(0, startEulerY + rotationDirection * totalRotationAmount, 0);
            Debug.Log($"Euler Y After: {pivot.transform.eulerAngles.y}");
        }
        
        isRotating = false;
    }

    void ProcessSnapToSelectedCharacter()
    {
        if (!isSnapping)
        {
            if (snapToEngineer.triggered)
            {
                SnapToEngineer();
            }
            else if (snapToHacker.triggered)
            {
                SnapToHacker();
            }
            else if (snapToLoader.triggered)
            {
                SnapToLoader();
            }
            else if (snapToScout.triggered)
            {
                SnapToScout();
            }
        }
    }

    public void SnapToEngineer()
    {
        SnapToCharacter(SurvivorManager.instance.GetSurvivor(
            SurvivorManager.ENGINEER_NAME));
    }

    public void SnapToLoader()
    {
        SnapToCharacter(SurvivorManager.instance.GetSurvivor(
                    SurvivorManager.LOADER_NAME));
    }

    public void SnapToHacker()
    {
        SnapToCharacter(SurvivorManager.instance.GetSurvivor(
            SurvivorManager.HACKER_NAME));
    }

    public void SnapToScout()
    {
        SnapToCharacter(SurvivorManager.instance.GetSurvivor(
            SurvivorManager.SCOUT_NAME));
    }

    private void SnapToCharacter(Survivor surv)
    {
        StartCoroutine(SnapCoroutine(surv));
        GameObject go = surv.inGameController.gameObject;
        SelectionManager.instance.HandleSelection(go, go.GetComponent<Selectable>());
        SelectionManager.instance.HighlightObject(go);
    }

    IEnumerator SnapCoroutine(Survivor surv)
    {
        isSnapping = true;
        float startTime = Time.time;
        Vector3 startPosition = pivot.transform.position;
        Vector3 snapPosition = surv.inGameController.transform.position;
        while (Time.time < startTime + snapTime)
        {
            float progress = (Time.time - startTime) / snapTime;
            pivot.transform.position = Vector3.Lerp(startPosition, snapPosition, progress);
            yield return null;
        }
        isSnapping = false;
    }
}
