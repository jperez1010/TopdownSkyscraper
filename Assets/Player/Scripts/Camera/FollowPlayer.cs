using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private GameObject target;
    public float distance;
    private Vector3 offset = new Vector3(Mathf.Sqrt(2) / 4, Mathf.Sqrt(3) / 2, -Mathf.Sqrt(2) / 4);
    public float playerHeight = 0.5f;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        this.transform.position = target.transform.position - distance * playerHeight * Vector3.up + distance * offset;

    }
}
