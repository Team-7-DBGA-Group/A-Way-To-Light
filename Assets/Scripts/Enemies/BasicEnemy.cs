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

    [Header("Basic Enemy References")]
    [SerializeField]
    private MeshRenderer eyesRenderer;
    [SerializeField]
    private Material glowMat;
    [SerializeField]
    private Material blackMat;

    private Transform _target;
    private NavMeshAgent _agent = null;

    // States
    private FollowingTargetState _followingTargetState;
    private EnemyAttackingState _enemyAttackingState;

    private bool _isStunned = false;

    private Animator _animator;

    public override void Attack()
    {
        if (!CanAttack)
            return;
        
        CustomLog.Log(CustomLog.CustomLogType.AI, "Attacking");
        _target.GetComponent<Actor>().TakeDamage(1);
        StartCoroutine(COStartAttackCooldown());
    }

    public override void Interact()
    {
        if (!IsAlive)
        {
            _agent.enabled = true;
            IsAlive = true;
            _animator.SetTrigger("Rise");
            eyesRenderer.material = glowMat;
            return;
        }

        // Stun
        if (_isStunned)
            return;

        StartCoroutine(COWaitStun());
    }

    protected override void Awake()
    {
        base.Awake();

        _target = FindObjectOfType<Player>().transform;
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        _agent.enabled = false;

        FSM = new FSMSystem();
        _followingTargetState = new FollowingTargetState(_target, _agent);
        _enemyAttackingState = new EnemyAttackingState(this);
        FSM.AddState(_followingTargetState);
        FSM.AddState(_enemyAttackingState);

        eyesRenderer.material = blackMat;
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

        if (Vector3.Distance(_target.position, this.transform.position) <= AttackRange && FSM.CurrentState != _enemyAttackingState)
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

    public override void Die()
    {
        CustomLog.Log(CustomLog.CustomLogType.GAMEPLAY, "Enemy " + this.gameObject.name + " Killed!");
        Destroy(gameObject);
    }
}
