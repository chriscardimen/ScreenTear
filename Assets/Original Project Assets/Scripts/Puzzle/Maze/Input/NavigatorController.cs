using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NavigatorController : MonoBehaviour
{
    public bool leftIsHeld;
    public bool rightIsHeld;
    public bool upIsHeld;
    public bool downIsHeld;
    public bool movementEnabled;

    public GameObject player;
    public float moveSpeed;
    private Rigidbody2D rigid;

    private void Start()
    {
        movementEnabled = true;
        leftIsHeld = false;
        rightIsHeld = false;
        upIsHeld = false;
        downIsHeld = false;
        rigid = player.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //Ensure that player doesn't rotate.
        player.transform.rotation = new Quaternion(0, 0, 0, 0);
        if (movementEnabled)
        {
            if (leftIsHeld)
            {
                MoveLeft();
            }

            if (rightIsHeld)
            {
                MoveRight();
            }

            if (upIsHeld)
            {
                MoveUp();
            }

            if (downIsHeld)
            {
                MoveDown();
            }

            if (!(leftIsHeld || rightIsHeld))
            {
                SlowXMovement();
            }

            if (!(upIsHeld || downIsHeld))
            {
                SlowYMovement();
            }
        }
    }

    public void MoveLeft()
    {
        player.transform.rotation = Quaternion.Euler(0, 180, 0);
        rigid.velocity = new Vector2(-moveSpeed, rigid.velocity.y);
    }

    public void MoveRight()
    {
        player.transform.rotation = Quaternion.Euler(0, 0, 0);
        rigid.velocity = new Vector2(moveSpeed, rigid.velocity.y);
    }

    public void MoveDown()
    {
        rigid.velocity = new Vector2(rigid.velocity.x, -moveSpeed);
    }

    public void MoveUp()
    {
        rigid.velocity = new Vector2(rigid.velocity.x, moveSpeed);
    }

    public void SlowXMovement()
    {
        rigid.velocity = new Vector2(0, rigid.velocity.y);
    }

    public void SlowYMovement()
    {
        rigid.velocity = new Vector2(rigid.velocity.x, 0);
    }


    public void ToggleLeft(InputAction.CallbackContext context)
    {
        leftIsHeld = !context.canceled;
    }

    public void ToggleRight(InputAction.CallbackContext context)
    {
        rightIsHeld = !context.canceled;
    }

    public void ToggleDown(InputAction.CallbackContext context)
    {
        downIsHeld = !context.canceled;
    }

    public void ToggleUp(InputAction.CallbackContext context)
    {
        upIsHeld = !context.canceled;
    }

    public void EnableMovement(bool enable)
    {
        movementEnabled = enable;
    }
}