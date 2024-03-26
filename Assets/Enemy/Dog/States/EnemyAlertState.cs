using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using Pathfinding;

public class EnemyAlertState : State<EnemyStateEnum>
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

    // Prefabs
    GameObject suprise;
    string suprisePrefabPath = "Enemy/Prefab/Alert Icon";

    public EnemyAlertState(EnemyStateEnum key) : base(key) { }

    public override void EnterState(GameObject gameObject)
    {
        tBuffer = 0.5f;
        tEnter = Time.time;
        exitable = false;
        queuedStateKey = stateKey;

        monster = gameObject;
        rb = monster.GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player");
        data = monster.GetComponent<DogData>();
        animator = monster.GetComponentInChildren<Animator>();
        suprise = GameObject.Instantiate(Resources.Load(suprisePrefabPath, typeof(GameObject))) as GameObject;
        suprise.transform.position = monster.transform.position + 3*Vector3.up;

        animator.CrossFade("Suprise", 0.01f);
    }

    public override void UpdateState(GameObject gameObject)
    {
        base.UpdateState(gameObject);
        queuedStateKey = EnemyStateEnum.ALERT;
        if (exitable)
        {
            GameObject.Destroy(suprise);
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
}