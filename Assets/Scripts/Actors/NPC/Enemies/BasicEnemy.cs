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

    private bool _isStop = false;

    public void StopEnemy()
    {
        Animator.ResetTrigger("Attack");
        Animator.SetFloat("MovementVelocity",0);
        _agent.enabled = false;
        _isStop = true;
    }
    public void ResumeEnemey()
    {
        _agent.enabled = true;
        _isStop = false;
    }

    public override void LoadData(GameData data)
    {
        bool isAlive = false;
        bool isDead = false;
        data.BasicEnemiesAlive.TryGetValue(ID, out isAlive);
        data.BasicEnemiesDead.TryGetValue(ID, out isDead);

        if (isDead)
        {
            this.gameObject.SetActive(false);
            return;
        }

        if (isAlive)
        {
            Interact();
            OutOfCombat();
        }       
    }

    public override void SaveData(GameData data)
    {
        if (data.BasicEnemiesAlive.ContainsKey(ID))
            data.BasicEnemiesAlive.Remove(ID);

        if(data.BasicEnemiesDead.ContainsKey(ID))
            data.BasicEnemiesDead.Remove(ID);

        data.BasicEnemiesAlive.Add(ID, IsAlive);
        data.BasicEnemiesDead.Add(ID, IsDead);
    }

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
        //Destroy(gameObject);
        this.gameObject.SetActive(false);
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
        Player.OnPlayerDie += OutOfCombat;
        GameManager.OnPause += HandlePause;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        SpawnManager.OnPlayerSpawn -= SetupFSM;
        Player.OnPlayerDie -= OutOfCombat;
        GameManager.OnPause -= HandlePause;
    }

    protected override void Update()
    {
        base.Update();

        if (!IsAlive)
            return;

        if (_target == null)
            return;

        if (_isStop)
            return;
        
        // If not in combat
        Enemy enemy = null;
        if(!EnemyManager.Instance.InCombatEnemies.TryGetValue(this.GetHashCode(), out enemy))
        {
            CheckBackInCombat();
            return;
        }

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

    private void OutOfCombat()
    {
        EnemyManager.Instance.DeregisterInCombatEnemy(this.GetHashCode());
    }

    private void CheckBackInCombat()
    {
        if(Vector3.Distance(_target.position, this.transform.position) <= CombatRange)
        {
            EnemyManager.Instance.RegisterInCombatEnemy(this.GetHashCode(), this);
        }
    }

    private void HandlePause(bool isPause)
    {
        if (isPause)
            StopEnemy();
        else
            ResumeEnemey();
    }
}
