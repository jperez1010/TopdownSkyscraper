using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonUtility
{
    public static void MoveObject(GameObject gameObject, Vector3 movement, float speed, Rigidbody rb)
    {
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
}
