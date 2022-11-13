using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Utils;

public class BasicEnemy : Enemy
{
    [Header("Basic Enemy Settings")]
    [SerializeField]
    private float stunDuration = 1.5f;

    private Transform _target = null;
    private NavMeshAgent _agent = null;

    // States
    private FollowingTargetState _followingTargetState;
    private EnemyAttackingState _enemyAttackingState;

    private bool _isStunned = false;

    public override void Attack()
    {
        if (!CanAttack)
            return;
        
        CustomLog.Log(CustomLog.CustomLogType.AI, "Attacking");
        StartCoroutine(COStartAttackCooldown());
    }

    public override void Interact()
    {
        // Stun
        if (_isStunned)
            return;

        StartCoroutine(COWaitStun());
    }

    private void Awake()
    {
        _target = FindObjectOfType<Player>().transform;

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

    private IEnumerator COWaitStun()
    {
        _isStunned = true;
        _agent.isStopped = true;
        yield return new WaitForSeconds(stunDuration);
        _agent.isStopped = false;
        _isStunned = false;
    }
}
