using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimb : MonoBehaviour
{
    public bool WasClimbing { get; set; }
    public bool IsClimbing { get; private set; }

    [Header("Climbing Settings")]
    [SerializeField]
    private PlayerMovement playerMovementInstance = null;
    [SerializeField]
    private Transform climbCheck = null;
    [SerializeField]
    private Transform groundCheck = null;
    [SerializeField]
    private LayerMask climbableGroundCheck = new LayerMask();
    [SerializeField]
    private float climbDistance = 0.2f;
    [SerializeField]
    private float climbingSpeed = 2f;

    private bool _isTouchingClimbableWall;

    private bool _canClimb;
    private bool _mustFloat;
    private CharacterController _characterController;
    private Vector3 _lastClimbCheckPos;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        WasClimbing = false;
        _canClimb = true;
        _mustFloat = false;
    }

    void Update()
    {   
        _isTouchingClimbableWall = Physics.CheckSphere(climbCheck.position, climbDistance, climbableGroundCheck);
        if (_canClimb)
        {
            if (_isTouchingClimbableWall && !playerMovementInstance.IsGrounded)
            {
                IsClimbing = true;
                WasClimbing = true;
                if (Input.GetKey(KeyCode.W))
                {
                    _characterController.Move(Vector3.up * climbingSpeed * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    _characterController.Move(Vector3.down * climbingSpeed * Time.deltaTime);
                }
            }
            else
            {
                if (WasClimbing)
                {
                    _canClimb = false;
                    _lastClimbCheckPos = climbCheck.position;
                    _mustFloat = true;
                }
                else
                    IsClimbing = false;
            }
        }

        if (_mustFloat)
        {
            if (!playerMovementInstance.IsGrounded)
            {
                if (groundCheck.transform.position.y <= _lastClimbCheckPos.y)
                {
                    transform.Translate(transform.up * 2 * Time.deltaTime);
                }
                transform.Translate(transform.worldToLocalMatrix.MultiplyVector(transform.forward) * 1 * Time.deltaTime);
            }
            else
            {
                WasClimbing = false;
                _mustFloat = false;
                _canClimb = true;
            }
        }
    }
}
