using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private PlayerAim playerAim;
    [SerializeField]
    private PlayerClimb playerClimb;
    [SerializeField]
    private PlayerMovement playerMovement;

    [SerializeField]
    private GameObject playerAimCamera;
    [SerializeField]
    private GameObject playerClimbCamera;
    [SerializeField]
    private GameObject playerMovementCamera;
    // Ha più senso? 
    // [SerializeField]
    // private GameObject[] Cameras;

    private void Start()
    {
        playerAimCamera.SetActive(false);
        playerClimbCamera.SetActive(false);
        playerMovementCamera.SetActive(true);
    }

    private void Update()
    {
        if (playerAim.IsAiming)
        {
            playerAimCamera.SetActive(true);
            playerClimbCamera.SetActive(false);
            playerMovementCamera.SetActive(false);
        }
        else if (playerClimb.IsClimbing)
        {
            playerAimCamera.SetActive(false);
            playerClimbCamera.SetActive(true);
            playerMovementCamera.SetActive(false);
        }
        else
        {
            playerAimCamera.SetActive(false);
            playerClimbCamera.SetActive(false);
            playerMovementCamera.SetActive(true);
        }
    }
}
