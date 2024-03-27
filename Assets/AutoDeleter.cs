using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDeleter : MonoBehaviour
{
    public float counter = 1.5f;
    private float tempcounter;
    // Start is called before the first frame update
    void Start()
    {
        tempcounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        tempcounter += Time.deltaTime;
    }
}
