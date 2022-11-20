using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAim : MonoBehaviour
{
    public static event Action OnAimActive;
    public static event Action OnAimInactive;

    public bool IsAiming { get; private set; }

    [Header("References")]
    [SerializeField]
    private Transform mainCameraTransform = null;

    [Header("Aim Camera Settings")]
    [SerializeField]
    private float turnSmoothTime = 0.1f;

    private float _turnSmoothVelocity;
    private bool _canAim = true;

    private void OnEnable()
    {
        PlayerClimb.OnClimbingEnter += () => { _canAim = false; };
        PlayerClimb.OnClimbingExit += () => { _canAim = true; };

        DialogueManager.OnDialogueEnter += () => { _canAim = false; };
        DialogueManager.OnDialogueExit += () => { _canAim = true; };
    }

    private void Update()
    {
        if (!_canAim)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            IsAiming = true;
            OnAimActive?.Invoke();
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            IsAiming = false;
            OnAimInactive?.Invoke();
        }

        if(IsAiming)
        {
            // Calcolo rotazione player tenendo conto di dove sta guardando la camera
            float targetAngle = Mathf.Atan2(Vector3.zero.x, Vector3.zero.z) * Mathf.Rad2Deg + mainCameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }
}
