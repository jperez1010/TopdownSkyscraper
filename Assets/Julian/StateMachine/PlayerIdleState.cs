using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerIdleState : State<PlayerStateEnum>
{
    Animator animator;
    private String animation = "Idle";

    public PlayerIdleState(PlayerStateEnum key) : base(key) { }

    public override void EnterState(GameObject gameObject)
    {
        tBuffer = 0;
        exitable = true;
        queuedStateKey = stateKey;

        animator = gameObject.GetComponent<Animator>();


        Debug.Log("Finna play idle");
        animator.Play(animation);
    }
    public override void UpdateState(GameObject gameObject)
    {
        if (MovementKeyDown())
        {
            Debug.Log("Key pressed");
        }
        if (AttackKeyDown())
        {
            queuedStateKey = PlayerStateEnum.ATTACK;
        }
        if (MovementKeyDown())
        {
            if (RunKeyDown())
            {
                queuedStateKey = PlayerStateEnum.RUN;
            }
            else
            {
                queuedStateKey = PlayerStateEnum.WALK;
            }
        }
    }
    private bool MovementKeyDown()
    {
        return Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D);
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
