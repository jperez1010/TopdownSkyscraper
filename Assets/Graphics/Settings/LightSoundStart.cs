using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSoundStart : MonoBehaviour
{
    [SerializeField] private GameObject targetObject; 

    private void Awake()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.instance.lightBuzz, targetObject);
    }
}
