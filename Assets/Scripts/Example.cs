using System.Collections;
using System.Reflection;
using UnityEngine;

public class Example : MonoBehaviour
{
    private void Awake()
    {
        finiteStateMachine = gameObject.AddComponent<FiniteStateMachineRunner>().Initialize<State>(this, State.Idle);
    }

    private IEnumerator OnAttackStateEnter()
    {
        Debug.Log($"{GetType()}.{nameof(OnAttackStateEnter)}");
        while (true)
        {
            yield return null;
            Debug.Log($"Attack");
        }
    }

    private void OnIdleStateEnter()
    {
        Debug.Log($"{GetType()}.{MethodBase.GetCurrentMethod().Name}");
    }

    private void OnIdleStateUpdate()
    {
        Debug.Log($"{GetType()}.{MethodBase.GetCurrentMethod().Name}");
        if (Time.time > 1)
        {
            finiteStateMachine.State = State.Attack;
        }
    }

    private void OnIdleStateExit()
    {
        Debug.Log($"{GetType()}.{MethodBase.GetCurrentMethod().Name}");
    }

    private enum State
    {
        Attack,
        Idle,
    }
    private FiniteStateMachine<State> finiteStateMachine;
}
