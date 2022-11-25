using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAnimations : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private PlayerMovement playerMovement;
    [SerializeField]
    private PlayerClimb playerClimb;
    [SerializeField]
    private Player player;

    private CharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        PlayerLightShooting.OnShot += PlayShotAnimation;
        PlayerCombat.OnAttack += PlayAttackAnimation;
        player.OnKnockbackEnter += PlayHitAnimation;
    }

    private void OnDisable()
    {
        PlayerLightShooting.OnShot -= PlayShotAnimation;
        PlayerCombat.OnAttack -= PlayAttackAnimation;
        player.OnKnockbackEnter -= PlayHitAnimation;
    }

    private void LateUpdate()
    {
        animator.SetBool("IsGrounded", playerMovement.IsGrounded);
        animator.SetFloat("MovementVelocity", _characterController.velocity.magnitude / 10);
        animator.SetBool("IsClimbing", playerClimb.IsClimbing);
        animator.SetFloat("MovementY", Input.GetAxis("Vertical"));
    }

    private void PlayAttackAnimation() => animator.SetTrigger("Attack");
    private void PlayHitAnimation() => animator.SetTrigger("Hit");
    private void PlayShotAnimation() => animator.SetTrigger("Shot");
}
