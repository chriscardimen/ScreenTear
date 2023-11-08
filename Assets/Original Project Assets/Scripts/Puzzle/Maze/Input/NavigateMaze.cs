using System;
using System.Collections;
using System.Collections.Generic;
// using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class NavigateMaze : MonoBehaviour
{
    private Vector2 pos;
    private bool colliding;

    private void Start()
    {
        colliding = false;
        pos = new Vector2();
    }

    private void FixedUpdate()
    {
        this.transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    public void CollisionOff()
    {
        colliding = false;
    }
    
    public void Move()
    {

        
        pos = Mouse.current.position.ReadValue();
        
        if (!colliding)
        {
            this.transform.position = pos;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        colliding = true;
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        colliding = true;
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        colliding = false;
    }
}