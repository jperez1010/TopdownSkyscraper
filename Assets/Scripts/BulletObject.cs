using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObject : MonoBehaviour
{
    public Vector3 direction;
    const float bulletSpeed = 100f;
    public float timer;
    public int dmg;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * bulletSpeed*Time.deltaTime;
        if (timer < 2f)
        {
            timer += Time.deltaTime;
        } else
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
