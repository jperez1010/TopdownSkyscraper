using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerWalkState : State<PlayerStateEnum>
{
    private Animator animator;
    private Rigidbody rb;
    protected String animation = "Walking";
    
    public float speed = 8f;
    public float angularSpeed = 3f;

    Vector3 m_EulerAngleVelocity = new Vector3(0,1,0);

    public PlayerWalkState(PlayerStateEnum key) : base(key) { }

    public override void EnterState(GameObject gameObject)
    {
        tBuffer = 0;
        exitable = true;
        queuedStateKey = stateKey;

        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();

        animator.CrossFade(animation, 0.01f);
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
        if (RunKeyDown())
        {
            queuedStateKey = PlayerStateEnum.RUN;
        }
    }

    protected void MovePlayer(GameObject gameObject)
    {
        Vector3 movement = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            movement += new Vector3(-speed * Mathf.Sin(Mathf.PI / 180 * 45), 0, speed * Mathf.Cos(Mathf.PI / 180 * 45));
        }
        if (Input.GetKey(KeyCode.D))
        {
            movement += new Vector3(speed * Mathf.Sin(Mathf.PI / 180 * 45), 0, speed * Mathf.Cos(Mathf.PI / 180 * 45));
        }
        if (Input.GetKey(KeyCode.A))
        {
            movement += new Vector3(-speed * Mathf.Sin(Mathf.PI / 180 * 45), 0, -speed * Mathf.Cos(Mathf.PI / 180 * 45));
        }
        if (Input.GetKey(KeyCode.S))
        {
            movement += new Vector3(speed * Mathf.Sin(Mathf.PI / 180 * 45), 0, -speed * Mathf.Cos(Mathf.PI / 180 * 45));
        }
        movement = movement.normalized;

        if (movement.magnitude > 0f)
        {
            float currentAngle = gameObject.transform.rotation.eulerAngles.y;
            float goalAngle = 180 / Mathf.PI * Mathf.Acos(movement.z);
            if (Mathf.Asin(movement.x) < 0)
            {
                goalAngle = 360 - goalAngle;
            }
            float finalAngle = Mathf.LerpAngle(currentAngle, goalAngle, Time.deltaTime * 5f);
            gameObject.transform.rotation = Quaternion.Euler(Vector3.up * finalAngle);

            float rotationMultiplier = Vector3.Dot(movement, new Vector3(Mathf.Sin(Mathf.PI / 180 * finalAngle), 0, Mathf.Cos(Mathf.PI / 180 * finalAngle)));
            rb.velocity = speed * rotationMultiplier * movement + rb.velocity.y * Vector3.up;
        }
    }

    protected bool MovementKeyReleased()
    {
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D)) {
            if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
            {
                return true;
            }
        }
        return false;
    }

    protected bool RunKeyDown()
    {
        return Input.GetKeyDown(KeyCode.LeftShift);
    }

    protected bool AttackKeyDown()
    {
        return Input.GetKeyDown(KeyCode.Mouse0);
    }
}

