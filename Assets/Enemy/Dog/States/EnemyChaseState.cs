using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyChaseState : State<EnemyStateEnum>
{
    // Refrences
    protected GameObject monster;
    protected Rigidbody rb;
    protected GameObject player;

    // Enemy Data
    protected DogData data;
    protected float health;

    // A*
    protected Path path;
    protected int currentWaypoint = 0;
    protected bool chasing = false;
    protected float waitTime = 0;
    protected Seeker seeker;
    protected GameObject PlayerDetected;

    // Animation
    Animator animator;

    public EnemyChaseState(EnemyStateEnum key) : base(key) { }
    public override void EnterState(GameObject gameObject)
    {
        tBuffer = 0;
        exitable = true;
        queuedStateKey = stateKey;

        monster = gameObject;
        rb = monster.GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player");
        data = monster.GetComponent<DogData>();
        seeker = monster.GetComponent<Seeker>();
        animator = monster.GetComponentInChildren<Animator>();

        animator.CrossFade(DogData.chaseAnimation, 0.01f);

        GeneratePath();
    }

    public override void UpdateState(GameObject gameObject)
    {
        if (chasing)
            ChasePlayer();
        //Debug.DrawLine(gameObject.transform.position, player.transform.position, Color.blue);
    }

    private void GeneratePath()
    {
        Vector3 origin = rb.position;
        Vector3 target = player.transform.position;

        seeker.StartPath(origin, target, OnPathComplete);
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
            chasing = true;
        }
    }
    private void OnDestinationReached()
    {
        path = null;
        chasing = false;
        queuedStateKey = EnemyStateEnum.IDLE;
    }


    private void ChasePlayer()
    {
        if (path == null)
        {
            return;
        }
        if (currentWaypoint >= path.vectorPath.Count)
        {
            OnDestinationReached();
            return;
        }
        if (Vector3.Distance(path.vectorPath[path.vectorPath.Count-1], player.transform.position) > DogData.maxDiscrepency) 
        {
            GeneratePath();
            return;
        }

        Vector3 movement = path.vectorPath[currentWaypoint] - rb.position;
        movement -= Vector3.up * movement.y;
        CommonUtility.MoveObject(monster, movement, DogData.chaseSpeed, rb);

        if (movement.magnitude < DogData.nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            GameObject.Destroy(monster);
        }
    }
}
