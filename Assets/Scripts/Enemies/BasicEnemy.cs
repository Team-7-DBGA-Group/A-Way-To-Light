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
    private EnemyAttackingState _enemyAttackingState;

    public override void Attack()
    {
        Debug.Log("Attacking");
    }

    private void Awake()
    {
        // Delete this - Get player in another way
        _target = target;

        _agent = GetComponent<NavMeshAgent>();

        FSM = new FSMSystem();
        _followingTargetState = new FollowingTargetState(_target, _agent);
        _enemyAttackingState = new EnemyAttackingState(this);
        FSM.AddState(_followingTargetState);
        FSM.AddState(_enemyAttackingState);
    }


    private void Update()
    {
        if(Vector3.Distance(_target.position, this.transform.position) <= AttackRange && FSM.CurrentState != _enemyAttackingState)
        {
            FSM.GoToState(_enemyAttackingState);
        }
        else if (Vector3.Distance(_target.position, this.transform.position) >= AttackRange && FSM.CurrentState != _followingTargetState)
        {
            FSM.GoToState(_followingTargetState);
        }

        FSM.CurrentState.OnUpdate();
    }
}
