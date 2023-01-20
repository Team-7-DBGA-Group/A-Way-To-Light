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
    private FieldOfView fov;
    [SerializeField]
    private Player player;

    [Header("Attack Settings")]
    [SerializeField]
    private float attackCooldown = 3f;

    private bool _canAttack = true;

    public void AnimationFinished()
    {
        playerMovement.CanMove = true;
    }

    private void OnEnable()
    {
        GameManager.OnPause += HandlePause;
    }

    private void OnDisable()
    {
        GameManager.OnPause -= HandlePause;
    }

    private void Update()
    {
        if (!playerAim.IsAiming &&
            !playerClimb.IsClimbing &&
            playerMovement.IsGrounded &&
            _canAttack 
            && player.IsWeaponEquip 
            && !DialogueManager.Instance.IsDialoguePlaying
            && InputManager.Instance.GetFirePressed())
        {
            playerMovement.CanMove = false;
            if (fov && fov.CanSeeActor && fov.ActorToFollow != null)
                transform.LookAt(fov.ActorToFollow.transform);
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

    private void HandlePause(bool isPause)
    {
        if (isPause)
            _canAttack = false;
        else
            _canAttack = true;
    }
}
