using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    public State currentState { get; protected set; } = null;
    public State previousState { get; protected set; } = null;


    public void ChangeState(State newState)
    {
        currentState?.Exit();
        previousState = currentState;

        currentState = newState;
        currentState?.Enter();
    }


    void Update()
    {
        currentState.Update();
    }
}
