using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    public Rigidbody rigidbody;
    public float speed = 1f;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, speed * 1);
        }
        if (Input.GetKey(KeyCode.R))
        {
            rigidbody.velocity = Vector3.zero;
            transform.position = new Vector3(0, -0.83f, -6.89f);
        }
        Vector3 currentVelocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
        animator.SetBool("isWalking", (currentVelocity.magnitude >= speed / 20f));
    }
}
