using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerWalkState
{
    protected float stamina = 100;
    protected float staminaRate = 1;


    public PlayerRunState(PlayerStateEnum key) : base(key) 
    {
        speed = 10f;
        animation = "Walking";
    }

    public override void EnterState(GameObject gameObject)
    {
        base.EnterState(gameObject);
    }

    public override void UpdateState(GameObject gameObject)
    {
        MovePlayer(gameObject);

        if (AttackKeyDown())
        {
            queuedStateKey = PlayerStateEnum.ATTACK;
        }
        if (MovementKeyReleased())
        {
            queuedStateKey = PlayerStateEnum.IDLE;
        }
        if (RunKeyUp())
        {
            queuedStateKey = PlayerStateEnum.WALK;
        }

        speed = Mathf.Clamp(speed - staminaRate * Time.deltaTime, 0, 100);
        if(speed == 0)
        {
            queuedStateKey = PlayerStateEnum.WALK;
        }
    }

    protected bool RunKeyUp()
    {
        return Input.GetKeyUp(KeyCode.LeftShift);
    }
}
