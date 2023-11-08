using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//https://gamedev.stackexchange.com/questions/67839/is-there-a-way-to-display-navmesh-agent-path-in-unity
public class SelectionIndicator : MonoBehaviour
{
    public LineRenderer line;
    public float lineWidth = 0.4f;
    public float height = 0.1f;
    public int radius = 90;
    public bool lighting = true;
    public Vector3 target;
    public NavMeshAgent agent;
    public SurvivorController controller;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.generateLightingData = lighting;
        line.numCapVertices = radius;
        line.numCornerVertices = radius;
        line.material = transform.parent.gameObject.GetComponentInChildren<MeshRenderer>().material;
        agent = transform.parent.gameObject.GetComponent<NavMeshAgent>();
        controller = transform.parent.gameObject.GetComponent<SurvivorController>();
    }

    void getPath()
    {
        Vector3 pos = transform.position;
        if (line.positionCount > 0)
        {
            line.SetPosition(0, new Vector3(pos.x, target.y+0.1f, pos.z));

        }
        //yield return new WaitForEndOfFrame();

        DrawPath(agent.path);
    }

    void DrawPath(NavMeshPath path)
    {
        if (path.corners.Length < 2)
            return;

        line.positionCount = path.corners.Length;
        //Debug.Log(line.positionCount);
        for (int i = 1; i < path.corners.Length; i++)
        {
            Vector3 pathCorners = path.corners[i];
            line.SetPosition(i, new Vector3(pathCorners.x, target.y+0.1f, pathCorners.z));
        }
    }

    void Update()
    {
        if (controller.currentAction == SurvivorController.SurvivorAction.interacting ||
        controller.currentAction == SurvivorController.SurvivorAction.standing)
        {
            ClearPath();
        }
        else
        {
            target = agent.destination;
            getPath();
        }

    }

    void ClearPath()
    {
        target = transform.position;
        line.positionCount = 0;
    }

}
