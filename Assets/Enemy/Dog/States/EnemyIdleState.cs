using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyIdleState : State<EnemyStateEnum>
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
    protected bool patrolling = false;
    protected float waitTime = 0;
    protected Seeker seeker;
    protected GameObject PlayerDetected;

    // Animation
    Animator animator;

    public EnemyIdleState(EnemyStateEnum key) : base(key) { }

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

        animator.CrossFade(DogData.idleAnimation, 0.01f);
        waitTime = ComputeWaitTime();
    }

    public override void UpdateState(GameObject gameObject)
    {
        if (!patrolling)
        {
            if (waitTime > 0f)
                waitTime -= Time.deltaTime;
            else
                GeneratePath();
        }
        else
        {
            Patrol();
        }
        DetectPlayer();
    }

    private void DetectPlayer()
    {
        queuedStateKey = EnemyStateEnum.IDLE;

        float maxDetectionDistance = DogData.maxDetectionDistance;
        float FOV = DogData.FOV;

        Vector3 enemyPosition = monster.transform.position;
        Vector3 targetPosition = player.transform.position;

        if (Vector3.Distance(enemyPosition, targetPosition) > maxDetectionDistance)
        {
            return;
        }
        if (Physics.Linecast(enemyPosition, targetPosition, LayerMask.GetMask("Obstacle")))
        {
            return;
        }
        
        Vector3 direction = monster.transform.forward;
       
        float dtheta = Mathf.Acos(Vector3.Dot(direction.normalized, (targetPosition - enemyPosition).normalized)) * 180/Mathf.PI;

        if (dtheta > FOV)
        {
            return;
        }
        queuedStateKey = EnemyStateEnum.CHASE;
    }

    private void GeneratePath()
    {
        float innerRadius = DogData.innerRadius;
        float outerRadius = DogData.outerRadius;
        Vector3 origin = monster.transform.position;

        float theta = Random.Range(0, 2 * Mathf.PI);
        Vector3 directon = new Vector3(Mathf.Cos(theta), 0, Mathf.Sin(theta));
        float magnitude = Random.Range(innerRadius, outerRadius);
        Vector3 target = magnitude * directon + origin;

        if (!Physics.Raycast(origin, directon, magnitude)) 
        {
            if (!Physics.CheckSphere(target, DogData.nextWaypointDistance, LayerMask.GetMask("Obstacle")))
            {
                seeker.StartPath(origin, target, OnPathComplete);
            }
        }
    }
    
    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
            patrolling = true; 
            animator.CrossFade(DogData.trotAnimation, 0.01f);
        }
    }

    private void OnDestinationReached()
    {
        path = null;
        patrolling = false;
        waitTime = ComputeWaitTime();
        animator.CrossFade(DogData.idleAnimation, 0.01f);
    }

    private void Patrol()
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

        Vector3 movement = path.vectorPath[currentWaypoint] - rb.position;

        CommonUtility.MoveObject(monster, movement, DogData.walkSpeed, rb);

        if (movement.magnitude < DogData.nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    private float ComputeWaitTime()
    {
        float rng = Random.value;
        float waitTime = -DogData.lambda * Mathf.Log(1 - rng);
        return waitTime;
    }
    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject.Destroy(monster);
        }
    }
}