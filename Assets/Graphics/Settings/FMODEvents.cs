using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Main Menu MUSIC")]
    [field: SerializeField] public EventReference BGM { get; set; }

    [field: Header("Ambience")]
    [field: SerializeField] public EventReference ambience { get; set; }

    [field: Header("light Buzz")]
    [field: SerializeField] public EventReference lightBuzz { get; set; }

    [field: Header("Button Clicked")]
    [field: SerializeField] public EventReference StartButton { get; set; }

    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD Events instance in the scene.");
        }
        instance = this;
    }


}
