using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAim : MonoBehaviour
{
    public static event Action OnAimActive;
    public static event Action OnAimInactive;

    public bool IsAiming { get; private set; }

    [SerializeField]
    private GameObject PlayerCamera;
    [SerializeField]
    private GameObject AimCamera;
    [SerializeField]
    private Transform MainCameraTransform = null;

    [SerializeField]
    private float TurnSmoothTime = 0.1f;

    private float _turnSmoothVelocity;

    private void Start()
    {
        PlayerCamera.SetActive(true);
        AimCamera.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            PlayerCamera.SetActive(false);
            AimCamera.SetActive(true);

            IsAiming = true;
            OnAimActive?.Invoke();
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            PlayerCamera.SetActive(true);
            AimCamera.SetActive(false);

            IsAiming = false;
            OnAimInactive?.Invoke();
        }

        if(IsAiming)
        {
            // Calcolo rotazione player tenendo conto di dove sta guardando la camera
            float targetAngle = Mathf.Atan2(Vector3.zero.x, Vector3.zero.z) * Mathf.Rad2Deg + MainCameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, TurnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }
}
