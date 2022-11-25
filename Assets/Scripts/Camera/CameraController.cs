using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Cameras References")]
    [SerializeField]
    private GameObject playerAimCamera;
    [SerializeField]
    private GameObject playerClimbCamera;
    [SerializeField]
    private GameObject playerMovementCamera;
    [SerializeField]
    private GameObject playerDialogueCamera;

    private void Start()
    {
        playerAimCamera.SetActive(false);
        playerClimbCamera.SetActive(false);
        playerMovementCamera.SetActive(true);
    }

    private void OnEnable()
    {
        DialogueManager.OnDialogueEnter += DialogueEnterCameras;
        DialogueManager.OnDialogueExit += DialogueExitCameras;
        PlayerAim.OnAimActive += AimActiveCameras;
        PlayerAim.OnAimInactive += AimInactiveCameras;
        PlayerClimb.OnClimbingEnter += ClimbingEnterCameras;
        PlayerClimb.OnClimbingExit += ClimbingExitCameras;
    }

    private void OnDisable()
    {
        DialogueManager.OnDialogueEnter -= DialogueEnterCameras;
        DialogueManager.OnDialogueExit -= DialogueExitCameras;
        PlayerAim.OnAimActive -= AimActiveCameras;
        PlayerAim.OnAimInactive -= AimInactiveCameras;
        PlayerClimb.OnClimbingEnter -= ClimbingEnterCameras;
        PlayerClimb.OnClimbingExit -= ClimbingExitCameras;
    }

    private void DialogueEnterCameras()
    {
        playerDialogueCamera.SetActive(true);
        playerMovementCamera.SetActive(false);
    }

    private void DialogueExitCameras()
    {
        playerDialogueCamera.SetActive(false);
        playerMovementCamera.SetActive(true);
    }

    private void AimActiveCameras()
    {
        playerAimCamera.SetActive(true);
        playerClimbCamera.SetActive(false);
        playerMovementCamera.SetActive(false);
    }

    private void AimInactiveCameras()
    {
        playerAimCamera.SetActive(false);
        playerMovementCamera.SetActive(true);
    }

    private void ClimbingEnterCameras()
    {
        playerAimCamera.SetActive(false);
        playerClimbCamera.SetActive(true);
        playerMovementCamera.SetActive(false);
    }

    private void ClimbingExitCameras()
    {
        playerClimbCamera.SetActive(false);
        playerMovementCamera.SetActive(true);
    }
}
