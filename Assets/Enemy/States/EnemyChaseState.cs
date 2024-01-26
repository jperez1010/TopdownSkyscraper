using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : State<EnemyStateEnum>
{
    private int speed;
    private GameObject player;
    protected MonsterData monsterData;

    public Rigidbody rb;

    public EnemyChaseState(EnemyStateEnum key) : base(key) { }
    public override void EnterState(GameObject gameObject)
    {
        tBuffer = 0;
        exitable = true;
        queuedStateKey = stateKey;

        monsterData = gameObject.GetComponent<MonsterData>();
        monsterData.ActivateChase();

        rb = gameObject.GetComponent<Rigidbody>();


        player = GameObject.FindWithTag("Player");
    }

    public override void UpdateState(GameObject gameObject)
    {
        chasePlayer(gameObject);
    }

    private void chasePlayer(GameObject enemyGameObject)
    {
        float speed = 5;
        Vector3 movement = player.transform.position - enemyGameObject.transform.position;
        movement.y = 0f;
        movement.Normalize();

        if (movement.magnitude > 0f)
        {
            float currentAngle = enemyGameObject.transform.rotation.eulerAngles.y;
            float goalAngle = 180 / Mathf.PI * Mathf.Acos(movement.z);
            if (Mathf.Asin(movement.x) < 0)
            {
                goalAngle = 360 - goalAngle;
            }
            float finalAngle = Mathf.LerpAngle(currentAngle, goalAngle, Time.deltaTime * 5f);
            enemyGameObject.transform.rotation = Quaternion.Euler(Vector3.up * finalAngle);

            float rotationMultiplier = Vector3.Dot(movement, new Vector3(Mathf.Sin(Mathf.PI / 180 * finalAngle), 0, Mathf.Cos(Mathf.PI / 180 * finalAngle)));
            rb.velocity = speed * rotationMultiplier * movement + rb.velocity.y * Vector3.up;
        }
    }


}
