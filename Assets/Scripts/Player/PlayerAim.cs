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
    private GameObject playerCamera;
    [SerializeField]
    private GameObject aimCamera;
    [SerializeField]
    private Transform mainCameraTransform = null;

    [Header("Aim Camera Settings")]
    [SerializeField]
    private float turnSmoothTime = 0.1f;

    private float _turnSmoothVelocity;

    private void Start()
    {
        playerCamera.SetActive(true);
        aimCamera.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            playerCamera.SetActive(false);
            aimCamera.SetActive(true);

            IsAiming = true;
            OnAimActive?.Invoke();
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            playerCamera.SetActive(true);
            aimCamera.SetActive(false);

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
