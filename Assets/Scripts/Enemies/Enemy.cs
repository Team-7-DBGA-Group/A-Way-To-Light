using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Actor, IInteractable
{
    public float AttackRange { get => attackRange; }
    public float CombatRange { get => combatRange; }

    [Header("Enemy References")]
    [SerializeField]
    private GameObject enemyUI = null;

    [Header("Enemy Settings")]
    [SerializeField]
    protected float AttackCooldown = 2.0f;
    [SerializeField]
    private float attackRange = 2.0f;
    [SerializeField]
    private float combatRange = 4.0f;
    [SerializeField]
    private LayerMask playerLayer = new LayerMask();
    
    protected bool CanAttack = true;
    protected bool IsAlive = false;

    // State Machine
    protected FSMSystem FSM;

    public abstract void Attack();

    public abstract void Interact();

    protected IEnumerator COStartAttackCooldown()
    {
        CanAttack = false;
        yield return new WaitForSeconds(AttackCooldown);
        CanAttack = true;
    }

    protected virtual void Awake()
    {
        // ...
    }

    protected override void Start()
    {
        base.Start();
        enemyUI.SetActive(false);
    }

    protected virtual void Update()
    {
        if (!IsAlive)
            return;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, combatRange, playerLayer);
        if(hitColliders.Length > 0)
        {
            enemyUI.SetActive(true);
        }
        else
        {
            enemyUI.SetActive(false);
        }
    }
}
