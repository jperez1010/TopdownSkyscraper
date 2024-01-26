using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class State<EState> where EState : Enum
{
    public float tBuffer;
    public float tEnter;
    public bool exitable;

    public EState stateKey;
    public EState queuedStateKey;

    public State(EState key)
    {
        stateKey = key;
    }

    // Basic State Fucntions
    public virtual void EnterState(GameObject gameObject)
    {
        
    }
    public virtual void ExitState(GameObject gameObject)
    {

    }
    public virtual void UpdateState(GameObject gameObject)
    {
        exitable = CountDownBuffer();
    }

    // Collision Detection
    public virtual void OnTriggerEnter(Collider other)
    {

    }
    public virtual void OnTriggerStay(Collider other)
    {

    }
    public virtual void OnTriggerExit(Collider other)
    {

    }

    // Get and Set
    public virtual EState GetNextStateKey()
    {
        return queuedStateKey;
    }

    // Private Methods
    private bool CountDownBuffer()
    {
        return (Time.time - tEnter) >= tBuffer;
    }
}
