using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IInteractable
{
    public float AttackRange { get => attackRange; }

    [Header("Enemy Settings")]
    [SerializeField]
    protected float AttackCooldown = 2.0f;
    [SerializeField]
    private float attackRange = 2.0f;
    
    protected bool CanAttack = true;

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
}
