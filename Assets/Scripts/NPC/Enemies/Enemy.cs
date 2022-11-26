using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : NPC
{
    public float AttackRange { get => attackRange; }
    public float CombatRange { get => combatRange; }

    [Header("Enemy References")]
    [SerializeField]
    private GameObject enemyUI = null;

    [Header("Enemy Weapon")]
    [SerializeField]
    private bool canDropWeapon = false;
    [SerializeField]
    private float dropRayDistance = 1.6f;
    [SerializeField]
    private GameObject wieldableWeaponPrefab = null;

    [Header("Enemy Settings")]
    [SerializeField]
    protected float AttackCooldown = 2.0f;
    [SerializeField]
    private float attackRange = 2.0f;
    [SerializeField]
    private float combatRange = 4.0f;
    [SerializeField]
    protected float StunDuration = 1.5f;
    [SerializeField]
    private LayerMask playerLayer = new LayerMask();

    protected bool CanAttack = true;
    protected bool IsStunned = false;

    // State Machine
    protected FSMSystem FSM;

    public abstract void Attack();
    public override void Interact()
    {
        if (!IsAlive)
        {
            Rise();
            return;
        }

        // Stun
        if (IsStunned)
            return;

        StartCoroutine(COWaitStun());
    }

    public override void Die()
    {
        DropWeapon();
    }


    public override void Rise()
    {
        base.Rise();
        SpawnWeapon();
    }

    protected override void Awake()
    {
        base.Awake();
        // ...
    }

    protected override void Start()
    {
        base.Start();
        enemyUI.SetActive(false);
    }

    protected virtual void OnEnable()
    {
        this.OnKnockbackEnter += HandleKnockbackEnter;
        this.OnKnockbackExit += HandleKnockbackExit;
    }

    protected virtual void OnDisable()
    {
        this.OnKnockbackEnter -= HandleKnockbackEnter;
        this.OnKnockbackExit -= HandleKnockbackExit;
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

        Debug.DrawRay(transform.position + (-gameObject.transform.forward * dropRayDistance) + new Vector3(0, 1f, 0), Vector3.down * 10.0f, Color.red);
    }
    protected IEnumerator COStartAttackCooldown()
    {
        CanAttack = false;
        yield return new WaitForSeconds(AttackCooldown);
        CanAttack = true;
    }

    protected IEnumerator COWaitStun()
    {
        IsStunned = true;
        yield return new WaitForSeconds(StunDuration);
        IsStunned = false;
    }

    private void SpawnWeapon()
    {
        WeaponSlot _weaponSlot = gameObject.GetComponentInChildren<WeaponSlot>();

        if (_weaponSlot == null)
            return;

        GameObject weaponObj = Instantiate(wieldableWeaponPrefab, _weaponSlot.transform);
        weaponObj.transform.localRotation = wieldableWeaponPrefab.transform.localRotation;
    }

    private void HandleKnockbackEnter()
    {
        IsStunned = true;
        Animator.SetTrigger("Hit");
    }

    private void HandleKnockbackExit() => IsStunned = false;

    private void DropWeapon()
    {
        if (!canDropWeapon)
            return;

        Weapon weapon = wieldableWeaponPrefab.GetComponent<Weapon>();
        RaycastHit hit;
        if (Physics.Raycast(transform.position + (-gameObject.transform.forward * dropRayDistance) + new Vector3(0, 1f, 0), Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            GameObject pickableObj = Instantiate(weapon.PickablePrefab, hit.point, Quaternion.identity);
            pickableObj.transform.forward = hit.collider.gameObject.transform.up;
            pickableObj.transform.position += new Vector3(0, 0.07f, 0);
        }
    }
}
