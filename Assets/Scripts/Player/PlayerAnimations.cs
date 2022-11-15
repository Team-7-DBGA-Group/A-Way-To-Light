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
    private PlayerLightShooting playerLightShooting;
    [SerializeField]
    private PlayerClimb playerClimb;

    private CharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        playerLightShooting.OnShot += () => { animator.SetTrigger("Shot"); };
    }

    private void LateUpdate()
    {
        animator.SetBool("IsGrounded", playerMovement.IsGrounded);
        animator.SetFloat("MovementVelocity", _characterController.velocity.magnitude / 10);
        animator.SetBool("IsClimbing", playerClimb.IsClimbing);
        animator.SetFloat("MovementY", Input.GetAxis("Vertical"));
    }
}
