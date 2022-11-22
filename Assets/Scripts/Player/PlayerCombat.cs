using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCombat : MonoBehaviour
{
    public static event Action OnAttack;

    [Header("References")]
    [SerializeField]
    private PlayerAim playerAim;
    [SerializeField]
    private PlayerClimb playerClimb;
    [SerializeField]
    private PlayerMovement playerMovement;

    [Header("Attack Settings")]
    [SerializeField]
    private float attackCooldown = 3f;

    private bool _canAttack = true;

    public void AnimationFinished()
    {
        playerMovement.CanMove = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !playerAim.IsAiming && !playerClimb.IsClimbing && playerMovement.IsGrounded && _canAttack /* && IsWeaponEquip */)
        {
            playerMovement.CanMove = false;
            Attack();
        }
    }

    private void Attack()
    {
        StartCoroutine(COStartBetweenShotCooldown());
        OnAttack?.Invoke();
    }

    private IEnumerator COStartBetweenShotCooldown()
    {
        _canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        _canAttack = true;
    }
}
