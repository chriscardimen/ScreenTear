using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Rail : MonoBehaviour
{
    private List<Vector3> nodes;
    public List<Transform> nodeTransforms;
    public int nodeCount;

    void Start()
    {
        nodes = new List<Vector3>();
        foreach (Transform transform in nodeTransforms){
            nodes.Add(transform.position);
        }
        nodeCount = nodes.Count;
    }

    public Vector3 ProjectPositionOnRail(Vector3 pos, int closestNodeIndex)
    {
        //int closestNodeIndex = GetClosestNode(pos);

        if (closestNodeIndex == 0)
        {
            Debug.DrawLine(pos, nodes[0], Color.yellow);

            return ProjectOnSegment(nodes[0], nodes[1], pos);
        }
        else if (closestNodeIndex == (nodeCount - 1))
        {
            Debug.DrawLine(pos, nodes[nodeCount - 1], Color.yellow);

            return ProjectOnSegment(nodes[nodeCount - 1], nodes[nodeCount - 2], pos);
        }
        else
        {
            Vector3 leftSeg = ProjectOnSegment(nodes[closestNodeIndex - 1], nodes[closestNodeIndex], pos);
            Vector3 rightSeg = ProjectOnSegment(nodes[closestNodeIndex + 1], nodes[closestNodeIndex], pos);
            Debug.DrawLine(pos, leftSeg, Color.red);
            Debug.DrawLine(pos, rightSeg, Color.blue);

            if ((pos - leftSeg).sqrMagnitude <= (pos - rightSeg).sqrMagnitude)
            {
                return leftSeg;
            }
            else
            {
                return rightSeg;
            }

        }
    }

    public Vector3 ProjectOnSegment(Vector3 v1, Vector3 v2, Vector3 pos)
    {
        Vector3 v1ToPos = pos - v1;
        Vector3 segDirection = (v2 - v1).normalized;

        float distanceFromV1 = Vector3.Dot(segDirection, v1ToPos);
        if (distanceFromV1 < 0.0f)
        {
            return v1;
        }
        else if (distanceFromV1 * distanceFromV1 > (v2 - v1).sqrMagnitude)
        {
            return v2;
        }
        else
        {
            Vector3 fromV1 = segDirection * distanceFromV1;
            return v1 + fromV1;
        }
    }

    public int GetClosestNode(Vector3 post)
    {
        int closestNodeIndex = -1;
        float shortestDistance = 0.0f;

        for (int i = 0; i < nodeCount; i++)
        {
            float sqrDistance = (nodes[i] - post).sqrMagnitude;
            if (i == 0 || (sqrDistance < shortestDistance))
            {
                shortestDistance = sqrDistance;
                closestNodeIndex = i;
            }
        }

        return closestNodeIndex;
    }

    private void Update()
    {
        if (nodeCount > 1)
        {
            for (int i = 0; i < nodeCount - 1; i++)
            {
                Debug.DrawLine(nodes[i], nodes[i + 1], Color.cyan);
            }
        }

    }

    [Button]
    private void AddTransformsToNodeList(){
        nodeTransforms.Clear();
        for (int i = 0; i < transform.childCount; i++){
            nodeTransforms.Add(transform.GetChild(i).transform);
        }
    }

    public List<Vector3> GetNodes(){
        return nodes;
    }

    public Vector3 GetNodeAt(int index){
        return nodes[index];
    }

    public void SetNodeAt(int index, Vector3 setTo){
        nodes[index] = setTo;
    }


}
