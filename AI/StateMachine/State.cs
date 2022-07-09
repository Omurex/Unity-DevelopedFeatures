using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected StateMachine owner;


    public State(StateMachine _owner)
    {
        owner = _owner;
    }


    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
