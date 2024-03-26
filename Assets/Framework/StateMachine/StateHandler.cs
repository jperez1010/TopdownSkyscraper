using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class StateHandler<EState> : MonoBehaviour where EState : Enum
{
    public Dictionary<EState, State<EState>> states = new Dictionary<EState, State<EState>>();

    [SerializeField] EState currentStateEnum;
    protected State<EState> currentState;
    protected State<EState> baseState;

    protected bool isTransitioning;

    // Basic Unity Functions
    private void Start()
    {
        currentState = baseState;
        isTransitioning = false;
        currentState.EnterState(gameObject);
        currentStateEnum = baseState.GetNextStateKey();
    }
    private void Update()
    {
        if (isTransitioning)
        {
            return;
        }
        currentState.UpdateState(gameObject);

        EState nextStateKey = currentState.GetNextStateKey();

        if (!nextStateKey.Equals(currentState.stateKey) && currentState.exitable)
        {
            currentStateEnum = nextStateKey;
            TransitionState(nextStateKey);
        }
    }

    // Collision Detection
    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
    }
    private void OnTriggerStay(Collider other)
    {
        currentState.OnTriggerStay(other);
    }
    private void OnTriggerExit(Collider other)
    {
        currentState.OnTriggerExit(other);
    }

    // Helper Functions
    private void TransitionState(EState nextStateKey)
    {
        isTransitioning = true;
        currentState.ExitState(gameObject);
        currentState = states[nextStateKey];
        currentState.EnterState(gameObject);
        isTransitioning = false;
    }
}
