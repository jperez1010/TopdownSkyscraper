using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyLungeState : State<EnemyStateEnum>
{
    // Refrences
    protected GameObject monster;
    protected Rigidbody rb;
    protected GameObject player;

    // Enemy Data
    protected DogData data;
    protected float health;

    // Animation
    Animator animator;

    public EnemyLungeState(EnemyStateEnum key) : base(key) { }

    public override void EnterState(GameObject gameObject)
    {
        tBuffer = 1f;
        tEnter = Time.time;
        exitable = false;
        queuedStateKey = stateKey;

        monster = gameObject;
        rb = monster.GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player");
        data = monster.GetComponent<DogData>();
        animator = monster.GetComponentInChildren<Animator>();

        animator.CrossFade("Suprise", 0.01f);
        rb.AddRelativeForce(1300f * (Vector3.up * 0.4f));
        SetLaunchVelocity();
    }

    public override void UpdateState(GameObject gameObject)
    {
        base.UpdateState(gameObject);
        if (Time.time - tEnter > 0.28f) {
            rb.AddRelativeForce(1 * rb.mass * Physics.gravity);
        } else
        {
            SetLaunchVelocity();
        }
        if (exitable)
        {
            queuedStateKey = EnemyStateEnum.CHASE;
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Weapon")
        {
            GameObject.Destroy(monster);
        }
    }
    
    private void SetLaunchVelocity()
    {
        Vector3 velocity = 20 * monster.transform.forward;
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;
    }
}
