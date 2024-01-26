using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    new public Camera camera;
    
    void Start(){
        camera = Camera.main;
    }

    private void LateUpdate()
    {
        transform.forward = camera.transform.forward;
    }
}
