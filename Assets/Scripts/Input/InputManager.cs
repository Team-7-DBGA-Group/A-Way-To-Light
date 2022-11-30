using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : Singleton<InputManager>
{
    // Attributes for long press
    public Vector2 MoveDirectionPlayer { get; private set; }
    public bool IsMovingPlayer { get; private set; }
    public bool IsAimPressedDown { get; private set; }
    public bool IsAimPressedUp { get; private set; }
    public bool IsRunningPressed { get; private set; }

    // Attributes for single press
    private bool _isFirePressed = false;
    private bool _isJumpPressed = false;
    private bool _isContinueDialoguePressed = false;
    private bool _isInteractPressed = false;

    // Methods for InputAction Events
    public void MovePressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsMovingPlayer = true;
            MoveDirectionPlayer = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            IsMovingPlayer = false;
            MoveDirectionPlayer = context.ReadValue<Vector2>();
        }
    }

    public void FirePressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isFirePressed = true;
        }
        else if (context.canceled)
        {
            _isFirePressed = false;
        }
    }

    public void AimPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsAimPressedDown = true;
            IsAimPressedUp = false;
        }
        else if (context.canceled)
        {
            IsAimPressedDown = false;
            IsAimPressedUp = true;
        }
    }

    public void RunPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsRunningPressed = true;
        }
        else if (context.canceled)
        {
            IsRunningPressed = false;
        }
    }
    public void JumpPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isJumpPressed = true;
        }
        else if (context.canceled)
        {
            _isJumpPressed = false;
        }
    }

    public void InteractPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isInteractPressed = true;
        }
        else if (context.canceled)
        {
            _isInteractPressed = false;
        }
    }

    public void ContinueDialoguePressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isContinueDialoguePressed = true;
        }
        else if (context.canceled)
        {
            _isContinueDialoguePressed = false;
        }
    }

    // Method for GetKey (one single press)
    public bool GetFirePressed()
    {
        bool result = _isFirePressed;
        _isFirePressed = false;
        return result;
    }

    public bool GetJumpPressed()
    {
        bool result = _isJumpPressed;
        _isJumpPressed = false;
        return result;
    }

    public bool GetContinueDialoguePressed()
    {
        bool result = _isContinueDialoguePressed;
        _isContinueDialoguePressed = false;
        return result;
    }

    public bool GetInteractPressed()
    {
        bool result = _isInteractPressed;
        _isInteractPressed = false;
        return result;
    }

}
