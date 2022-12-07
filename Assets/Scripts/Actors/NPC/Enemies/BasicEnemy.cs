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

    public void StopMovement()
    {
        _agent.isStopped = true;
    }

    public override void Attack()
    {
        if (!CanAttack)
            return;
        if (IsStunned)
            return;
        if (_target.GetComponent<PlayerClimb>().IsClimbing)
            return;

        CustomLog.Log(CustomLog.CustomLogType.AI, "Attacking");

        Animator.SetTrigger("Attack");
        StartCoroutine(COStartAttackCooldown());
    }

    public override void Die()
    {
        base.Die();
        CustomLog.Log(CustomLog.CustomLogType.GAMEPLAY, "Enemy " + this.gameObject.name + " Killed!");
        EnemyManager.Instance.DeregisterInCombatEnemy(this.GetHashCode());
        Destroy(gameObject);
    }
    public void StopDealingDamage()
    {
        GetComponentInChildren<WeaponDamageDealer>().EndDealDamage();
    }

    public void DealDamage()
    {
        GetComponentInChildren<WeaponDamageDealer>().StartDealDamage();
    }

    protected override void Awake()
    {
        base.Awake();

        //_target = FindObjectOfType<Player>().transform;
        _agent = GetComponent<NavMeshAgent>();

        _agent.enabled = false;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        SpawnManager.OnPlayerSpawn += SetupFSM;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        SpawnManager.OnPlayerSpawn -= SetupFSM;
    }

    protected override void Update()
    {
        base.Update();

        if (!IsAlive)
            return;

        if (_target == null)
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

    private void SetupFSM(GameObject playerObj)
    {
        _target = playerObj.transform;

        FSM = new FSMSystem();
        _followingTargetState = new FollowingTargetState(_target, _agent);
        _enemyAttackingState = new EnemyAttackingState(this);

        FSM.AddState(_followingTargetState);
        FSM.AddState(_enemyAttackingState);
    }
}
