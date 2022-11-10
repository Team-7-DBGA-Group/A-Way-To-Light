using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemy : Enemy
{
    [Header("DELETE THIS - TESTING ONLY")]
    [SerializeField]
    private Transform target;

    private Transform _target = null;
    private NavMeshAgent _agent = null;

    // States
    private FollowingTargetState _followingTargetState;

    private void Awake()
    {
        // Delete this - Get player in another way
        _target = target;

        _agent = GetComponent<NavMeshAgent>();

        FSM = new FSMSystem();
        _followingTargetState = new FollowingTargetState(_target, _agent);
        FSM.AddState(_followingTargetState);
    }


    private void Update()
    {
        FSM.CurrentState.OnUpdate();
    }
}
