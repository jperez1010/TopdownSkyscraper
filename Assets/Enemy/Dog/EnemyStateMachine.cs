using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateHandler<EnemyStateEnum>
{
    private void Awake()
    {
        states.Add(EnemyStateEnum.IDLE, new EnemyIdleState(EnemyStateEnum.IDLE));
        states.Add(EnemyStateEnum.ALERT, new EnemyAlertState(EnemyStateEnum.ALERT));
        states.Add(EnemyStateEnum.CHASE, new EnemyChaseState(EnemyStateEnum.CHASE));
        states.Add(EnemyStateEnum.LUNGE, new EnemyLungeState(EnemyStateEnum.LUNGE));

        baseState = states[EnemyStateEnum.IDLE];
    }
}
