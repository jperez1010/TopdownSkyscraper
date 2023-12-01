using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    public Rigidbody rigidbody;
    public float speed = 1f;
    public float angularSpeed = 1f;
    public float runSpeed = 1f;

    public float stamina = 1f;
    public int staminaTimer = 200;
    public int currentStaminaTimer = 0;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float actualSpeed = speed;
        if (stamina > 0 && Input.GetKey(KeyCode.LeftShift))
        {
            stamina -= Time.deltaTime;
            stamina = Mathf.Clamp(stamina, 0, 1);
            actualSpeed *= runSpeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            currentStaminaTimer = staminaTimer;
        }
        if (currentStaminaTimer > 0)
        {
            currentStaminaTimer -= 1;
        }
        if (!Input.GetKey(KeyCode.LeftShift) && currentStaminaTimer == 0)
        {
            stamina += Time.deltaTime;
            stamina = Mathf.Clamp(stamina, 0, 1);
        }
        if (Input.GetKey(KeyCode.W))
        {
            rigidbody.velocity = new Vector3(actualSpeed * Mathf.Sin(Mathf.PI / 180 * transform.rotation.eulerAngles.y), rigidbody.velocity.y, actualSpeed * Mathf.Cos(Mathf.PI / 180 * transform.rotation.eulerAngles.y));
            Debug.Log((180 / Mathf.PI * transform.rotation.eulerAngles.y, Mathf.Sin(180 / Mathf.PI * transform.rotation.eulerAngles.y), rigidbody.velocity));
        }
        if (Input.GetKey(KeyCode.D))
        {
            rigidbody.angularVelocity = new Vector3(0, angularSpeed, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rigidbody.angularVelocity = new Vector3(0, -angularSpeed, 0);
        }
        if (Input.GetKey(KeyCode.R))
        {
            rigidbody.velocity = Vector3.zero;
            transform.position = new Vector3(0, -0.83f, -6.89f);
        }
        Vector3 currentVelocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
        animator.SetBool("isWalking", (currentVelocity.magnitude >= speed / 20f));
        animator.SetBool("isTurningRight", (Mathf.Abs(rigidbody.angularVelocity.y) >= angularSpeed / 5f));
        animator.SetBool("isShiftDown", Input.GetKey(KeyCode.LeftShift));
    }
}
