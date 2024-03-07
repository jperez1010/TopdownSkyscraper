using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStateMachine : StateHandler<PlayerStateEnum>
{
    private void Awake()
    {
        states.Add(PlayerStateEnum.IDLE, new PlayerIdleState(PlayerStateEnum.IDLE));
        states.Add(PlayerStateEnum.WALK, new PlayerWalkState(PlayerStateEnum.WALK));
        states.Add(PlayerStateEnum.RUN, new PlayerRunState(PlayerStateEnum.RUN));
        states.Add(PlayerStateEnum.ATTACK, new PlayerSwingAttackState(PlayerStateEnum.ATTACK));
        states.Add(PlayerStateEnum.RANGEATTACK, new PlayerRangeAttackState(PlayerStateEnum.RANGEATTACK));

        baseState = states[PlayerStateEnum.IDLE];
    }
}
