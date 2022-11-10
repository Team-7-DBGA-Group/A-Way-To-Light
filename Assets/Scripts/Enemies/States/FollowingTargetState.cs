using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowingTargetState : FSMState
{
    private Transform _target;
    private NavMeshAgent _agent;

    public FollowingTargetState(Transform target, NavMeshAgent agent)
    {
        _target = target;
        _agent = agent;
    }

    public void SetTarget(Transform target) => _target = target;

    public override void OnEnter()
    {
        base.OnEnter();
        if (_target == null)
            return;

        _agent.SetDestination(_target.position);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (_target == null)
            return;

        _agent.SetDestination(_target.position);
    }
}
