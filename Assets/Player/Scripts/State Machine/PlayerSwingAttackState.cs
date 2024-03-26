using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerSwingAttackState : State<PlayerStateEnum>
{
    Animator animator;
    private String animation = "Great Sword Slash";

    public PlayerSwingAttackState(PlayerStateEnum key) : base(key) { }

    public override void EnterState(GameObject gameObject)
    {
        tBuffer = 1.267f / 2f;
        tEnter = Time.time;
        exitable = false;

        queuedStateKey = stateKey;
        animator = gameObject.GetComponent<Animator>();
        animator.CrossFade(animation, 0.05f);
    }

    public override void UpdateState(GameObject gameObject)
    {
        base.UpdateState(gameObject);
        queuedStateKey = PlayerStateEnum.IDLE;

        if (AttackKeyDown())
        {
            queuedStateKey = PlayerStateEnum.ATTACK;
        }
        if (MovementKey())
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
    private bool MovementKey()
    {
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
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
