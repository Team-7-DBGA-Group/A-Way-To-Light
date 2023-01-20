using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerClimb : MonoBehaviour
{
    public static event Action OnClimbingEnter;
    public static event Action OnClimbingExit;

    public bool WasClimbing { get; set; }
    public bool IsClimbing { get; private set; }

    [Header("References")]
    [SerializeField]
    private PlayerMovement playerMovementInstance = null;
    [SerializeField]
    private Transform climbCheck = null;
    [SerializeField]
    private Transform groundCheck = null;
    [Header("Climbing Settings")]
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

    private bool _eventFlag = false;
    private Vector3 _lastNormal;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        WasClimbing = false;
        _canClimb = true;
        _mustFloat = false;
    }

    private void OnEnable()
    {
        GameManager.OnPause += HandlePause;
    }

    private void OnDisable()
    {
        GameManager.OnPause -= HandlePause;
    }

    void Update()
    {
        _isTouchingClimbableWall = Physics.CheckSphere(climbCheck.position, climbDistance, climbableGroundCheck);
        if (_canClimb)
        {
            if (_isTouchingClimbableWall && !playerMovementInstance.IsGrounded)
            {
                RaycastHit raycastHit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out raycastHit, Mathf.Infinity, climbableGroundCheck))
                    transform.rotation = Quaternion.LookRotation(-raycastHit.normal);
                    
                
                if(_eventFlag == false)
                {
                    // Event Enter
                    OnClimbingEnter?.Invoke();
                    _eventFlag = true;
                }


                IsClimbing = true;
                WasClimbing = true;
                
                if (Input.GetKey(KeyCode.W))
                    _characterController.Move(Vector3.up * climbingSpeed * Time.deltaTime);
                if (Input.GetKey(KeyCode.S))
                    _characterController.Move(Vector3.down * climbingSpeed * Time.deltaTime);
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
                {
                    IsClimbing = false;
                    if (_eventFlag == true)
                    {
                        // Event Exit
                        OnClimbingExit?.Invoke();
                        _eventFlag = false;
                    }
                }
                    
            }
        }

        if (_mustFloat)
        {
            if (!playerMovementInstance.IsGrounded)
            {
                RaycastHit hit;
                if (!Physics.Raycast(groundCheck.position, groundCheck.forward, out hit, 10f, climbableGroundCheck))
                {
                    WasClimbing = false;
                    _mustFloat = false;
                    _canClimb = true;
                }
                if (groundCheck.transform.position.y <= _lastClimbCheckPos.y)
                    transform.Translate(transform.up * 2 * Time.deltaTime);
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

    private void HandlePause(bool isPause)
    {
        if (isPause)
            _canClimb = false;
        else
            _canClimb = true;
    }
}
