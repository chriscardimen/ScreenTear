using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using Cinemachine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    private float cameraSpeed;
    private float C_cameraSpeedRatio = 0.001f;
    public int nodeAt;
    public float scrollsens = 0.25f, maxScroll, minScroll;
    public float moveSens;

    private Vector2 mousePos, startPosMiddle, directionPos;


    [SerializeField]
    private CinemachineVirtualCamera mainVCam;


    bool isDrag = false;
    bool isAngle = false;

    private void Start()
    {
        if (this.GetComponent<RailMover>() != null)
            this.transform.position = this.GetComponent<RailMover>().rail.nodeTransforms[0].position;
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        mousePos = context.action.ReadValue<Vector2>();
    }

    public void CameraInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            directionPos = context.ReadValue<Vector2>();
            if (directionPos.y != 0)
            {
                isAngle = true;
            }
            if (directionPos.x != 0)
            {
                isDrag = true;

            }
            //Debug.Log(directionPos);

        }
        if (context.canceled)
        {
            isDrag = false;
            isAngle = false;
        }
    }


    void Update()
    {
        if (isDrag)
            MoveCamera();
        if (isAngle)
            MoveCameraAngle();
    }

    private void MoveCamera()
    {
        Rail rail = this.gameObject.GetComponent<RailMover>().rail;
        nodeAt = rail.GetClosestNode(transform.position);

        if (directionPos.x > 0)
        {
            nodeAt -= 1;
            if (nodeAt < 0)
            {
                nodeAt = rail.nodeCount - 1;
            }
        }
        else if (directionPos.x < 0)
        {
            nodeAt += 1;
            if (nodeAt >= rail.nodeCount)
            {
                nodeAt = 0;
            }
        }

        if (directionPos.y > 0)
        {

        }
        else if (directionPos.y < 0)
        {

        }
        //Debug.Log(nodeAt);
        transform.position = Vector3.MoveTowards(transform.position, rail.GetNodeAt(nodeAt), 0.5f);
        //this.transform.position = rail.ProjectPositionOnRail(rail.GetNodeAt(nodeAt), nodeAt);
    }

    public void ScrollCamera(InputAction.CallbackContext context)
    {
        if (SelectionManager.instance.GetMousePos().x > Screen.width * 0.30)
        {
            float scrollval = context.ReadValue<float>();
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

    public void MoveCameraAngle()
    {
        GameObject lookAt = GetComponent<RailMover>().lookAt.gameObject;

        lookAt.transform.Translate(0, directionPos.y * 0.5f, 0);
    }

    public void RotateCameraLeft(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            RotateCameraMath(-15);
        }
    }

    public void RotateCameraRight(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            RotateCameraMath(15);
        }
    }

    private void RotateCameraMath(float angle)
    {
        Vector3 original_rotation = mainVCam.transform.rotation.eulerAngles;
        original_rotation.y += angle;
        mainVCam.transform.rotation = Quaternion.Euler(original_rotation);

    }


}
