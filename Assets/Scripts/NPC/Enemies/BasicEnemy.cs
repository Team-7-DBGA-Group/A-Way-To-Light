using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Utils;

public class BasicEnemy : Enemy
{
    private Transform _target;
    private NavMeshAgent _agent = null;

    // States
    private FollowingTargetState _followingTargetState;
    private EnemyAttackingState _enemyAttackingState;

    public override void Rise()
    {
        base.Rise();
        _agent.enabled = true;
        EnemyManager.Instance.RegisterInCombatEnemy(this.GetHashCode(), this);
    }

    public override void Attack()
    {
        if (!CanAttack)
            return;
        if (IsStunned)
            return;

        CustomLog.Log(CustomLog.CustomLogType.AI, "Attacking");

        Animator.SetTrigger("Attack");
        _target.GetComponent<Actor>().TakeDamage(0, this.gameObject);
        StartCoroutine(COStartAttackCooldown());
    }

    protected override void Awake()
    {
        base.Awake();

        _target = FindObjectOfType<Player>().transform;
        _agent = GetComponent<NavMeshAgent>();

        _agent.enabled = false;

        FSM = new FSMSystem();
        _followingTargetState = new FollowingTargetState(_target, _agent);
        _enemyAttackingState = new EnemyAttackingState(this);
        
        FSM.AddState(_followingTargetState);
        FSM.AddState(_enemyAttackingState);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (!IsAlive)
            return;

        // Stop Movement if stunned
        if (FSM.CurrentState == _followingTargetState)
        {
            if (IsStunned)
                _agent.isStopped = true;
            else
                _agent.isStopped = false;
        }
         
        Animator.SetFloat("MovementVelocity", _agent.velocity.magnitude / 10);

        if (Vector3.Distance(_target.position, this.transform.position) <= AttackRange && FSM.CurrentState != _enemyAttackingState)
        { 
            _agent.isStopped = true;
            FSM.GoToState(_enemyAttackingState);
        }
        else if (Vector3.Distance(_target.position, this.transform.position) >= AttackRange && FSM.CurrentState != _followingTargetState)
        {
            _agent.isStopped = false;
            FSM.GoToState(_followingTargetState);
        }

        FSM.CurrentState.OnUpdate();
    }

    public override void Die()
    {
        CustomLog.Log(CustomLog.CustomLogType.GAMEPLAY, "Enemy " + this.gameObject.name + " Killed!");
        EnemyManager.Instance.DeregisterInCombatEnemy(this.GetHashCode());
        Destroy(gameObject);
    }
}
