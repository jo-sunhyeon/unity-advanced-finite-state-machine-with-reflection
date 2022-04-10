using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachineRunner : MonoBehaviour
{
    private IFiniteStateMachine finiteStateMachine;

    public FiniteStateMachine<TState> Initialize<TState>(MonoBehaviour monoBehaviour, TState initialState) where TState : Enum
    {
        var finiteStateMachine = new FiniteStateMachine<TState>(monoBehaviour, initialState);
        this.finiteStateMachine = finiteStateMachine;
        return finiteStateMachine;
    }

    private void Update()
    {
        finiteStateMachine.Update();
    }
}
