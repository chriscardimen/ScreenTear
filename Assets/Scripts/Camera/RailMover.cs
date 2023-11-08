using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailMover : MonoBehaviour
{
    public Rail rail;
    public Transform lookAt;

    private void Update()
    {
        transform.LookAt(lookAt.transform, Vector3.up);
    }
}
