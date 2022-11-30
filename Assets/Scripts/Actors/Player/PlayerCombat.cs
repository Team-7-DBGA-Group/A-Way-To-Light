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
    [SerializeField]
    private Player player;

    [Header("Attack Settings")]
    [SerializeField]
    private float attackCooldown = 3f;

    private bool _canAttack = true;
    private FieldOfView _fov = null;

    public void AnimationFinished()
    {
        playerMovement.CanMove = true;
    }

    private void Start()
    {
        TryGetComponent<FieldOfView>(out _fov);
    }

    private void Update()
    {
        if (!playerAim.IsAiming &&
            InputManager.Instance.GetFirePressed() && 
            !playerClimb.IsClimbing &&
            playerMovement.IsGrounded &&
            _canAttack 
            && player.IsWeaponEquip 
            && !DialogueManager.Instance.IsDialoguePlaying)
        {
            playerMovement.CanMove = false;
            if (_fov && _fov.CanSeeActor)
                transform.LookAt(_fov.ActorToFollow.transform);
            Attack();
        }
    }

    private void Attack()
    {
        StartCoroutine(COStartBetweenAttackCooldown());
        OnAttack?.Invoke();
    }

    private IEnumerator COStartBetweenAttackCooldown()
    {
        _canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        _canAttack = true;
    }
}
