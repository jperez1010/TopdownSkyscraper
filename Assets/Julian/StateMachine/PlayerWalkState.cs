using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using System;

public class PlayerWalkState : State<PlayerStateEnum>
{
    private Animator animator;
    private Rigidbody rb;
    private String animation = "Walking";
    
    public float speed = 5f;
    public float angularSpeed = 3f;

    public PlayerWalkState(PlayerStateEnum key) : base(key) { }

    public override void EnterState(GameObject gameObject)
    {
        tBuffer = 0;
        exitable = true;
        queuedStateKey = stateKey;

        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();

        Debug.Log("Finna play walk");
       
        animator.Play(animation);
    }
    public override void UpdateState(GameObject gameObject)
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.velocity = new Vector3(speed * Mathf.Sin(Mathf.PI / 180 * gameObject.transform.rotation.eulerAngles.y), rb.velocity.y, speed * Mathf.Cos(Mathf.PI / 180 * gameObject.transform.rotation.eulerAngles.y));
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.angularVelocity = new Vector3(0, angularSpeed, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.angularVelocity = new Vector3(0, -angularSpeed, 0);
        }

        if (AttackKeyDown())
        {
            queuedStateKey = PlayerStateEnum.ATTACK;
        }
        if (MovementKeyReleased())
        {
            queuedStateKey = PlayerStateEnum.IDLE;
        }
        if (RunKeyDown())
        {
            queuedStateKey = PlayerStateEnum.RUN;
        }
    }

    private bool MovementKeyReleased()
    {
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D)) {
            if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
            {
                return true;
            }
        }
        return false;
    }

    private bool RunKeyDown()
    {
        return Input.GetKeyDown(KeyCode.LeftShift);
    }

    private bool AttackKeyDown()
    {
        return Input.GetKeyDown(KeyCode.Mouse0);
    }
}

