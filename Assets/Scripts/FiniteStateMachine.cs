using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class FiniteStateMachine<TState> : IFiniteStateMachine where TState : Enum
{
    public TState State
    {
        get
        {
            return state;
        }
        set
        {
            CallStateExit();
            state = value;
            CallStateEnter();
        }
    }
    private TState state;

    public FiniteStateMachine(MonoBehaviour monoBehaviour, TState initialState)
    {
        this.monoBehaviour = monoBehaviour;
        state = initialState;
        CacheMethod();
        CallStateEnter();
    }

    private void CacheMethod()
    {
        var methodInfos = monoBehaviour.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        foreach (var methodInfo in methodInfos)
        {
            foreach (TState state in Enum.GetValues(typeof(TState)))
            {
                if (methodInfo.Name == "On" + state + "StateEnter")
                {
                    if (methodInfo.ReturnType == typeof(IEnumerator))
                    {
                        enterFuncs[state] = (Func<IEnumerator>)Delegate.CreateDelegate(typeof(Func<IEnumerator>), monoBehaviour, methodInfo);
                    }
                    else
                    {
                        enterActions[state] = (Action)Delegate.CreateDelegate(typeof(Action), monoBehaviour, methodInfo);
                    }
                }
                else if (methodInfo.Name == "On" + state + "StateUpdate")
                {
                    updateActions[state] = (Action)Delegate.CreateDelegate(typeof(Action), monoBehaviour, methodInfo);
                }
                else if (methodInfo.Name == "On" + state + "StateExit")
                {
                    exitActions[state] = (Action)Delegate.CreateDelegate(typeof(Action), monoBehaviour, methodInfo);
                }
            }
        }
    }

    public void Update()
    {
        CallStateUpdate();
    }

    private void CallStateEnter()
    {
        if (enterFuncs.TryGetValue(state, out Func<IEnumerator> coroutine))
        {
            monoBehaviour.StartCoroutine(coroutine());
        }
        else if (enterActions.TryGetValue(state, out Action action))
        {
            action();
        }
    }

    private void CallStateUpdate()
    {
        if (updateActions.TryGetValue(state, out Action action))
        {
            action();
        }
    }

    private void CallStateExit()
    {
        if (exitActions.TryGetValue(state, out Action action))
        {
            action();
        }
    }

    private MonoBehaviour monoBehaviour;
    private Dictionary<TState, Action> enterActions = new Dictionary<TState, Action>();
    private Dictionary<TState, Action> updateActions = new Dictionary<TState, Action>();
    private Dictionary<TState, Action> exitActions = new Dictionary<TState, Action>();
    private Dictionary<TState, Func<IEnumerator>> enterFuncs = new Dictionary<TState, Func<IEnumerator>>();
}
