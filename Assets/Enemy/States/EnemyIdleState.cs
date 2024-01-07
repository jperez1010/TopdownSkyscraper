using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : State<EnemyStateEnum>
{
    protected GameObject player;
    private GameObject PlayerDetected;
    private float maxDetectionDistance = 20f; 
    private float FOV = 120f; 

    public EnemyIdleState(EnemyStateEnum key) : base(key) { }

    public override void EnterState(GameObject gameObject)
    {
        tBuffer = 0;
        exitable = true;
        queuedStateKey = stateKey;

        player = GameObject.FindWithTag("Player");

        MonsterData monsterData = gameObject.GetComponent<MonsterData>();
        monsterData.ActivateIdle();
    }

    public override void UpdateState(GameObject gameObject)
    {
        DetectPlayer(gameObject);
    }

    private void DetectPlayer(GameObject enemyGameObject)
    {
        bool playerDetected = false;

        RaycastHit playerHit;
        Vector3 direction = enemyGameObject.transform.forward;
        Vector3 enemyPosition = enemyGameObject.transform.position;

        if (Physics.Raycast(enemyPosition, direction, out playerHit, maxDetectionDistance))
        {
            if (playerHit.collider.CompareTag("Player"))
            {
                Vector3 targetDir = playerHit.transform.position - enemyPosition;
                float angle = Vector3.Angle(targetDir, enemyGameObject.transform.forward);

                if (angle < FOV * 0.5f)
                {
                    PlayerDetected = playerHit.collider.gameObject;
                    playerDetected = true;
                    Debug.Log("Player Detected!");
                    if (playerDetected == true)
                    {
                        queuedStateKey = EnemyStateEnum.CHASE;
                    }
                    
                }
            }
        }

        if (!playerDetected)
        {
            PlayerDetected = null;
            //Debug.Log("Player Not Detected!");
            if (playerDetected == false)
            {
                queuedStateKey = EnemyStateEnum.IDLE;
            }

        }
    }



}
