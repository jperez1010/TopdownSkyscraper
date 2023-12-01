using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject target;
    public Vector3 offset;
    public float distance;

    void Update()
    {
        this.transform.position = target.transform.position + distance * offset;  
    }
}
