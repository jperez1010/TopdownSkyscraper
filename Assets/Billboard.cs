using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Camera camera;
    private void LateUpdate()
    {
        transform.forward = camera.transform.forward;
    }
}
