using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [Header("Cameras References")]
    [SerializeField]
    private CinemachineFreeLook playerAimCamera;
    [SerializeField]
    private CinemachineClearShot playerClimbCamera;
    [SerializeField]
    private CinemachineFreeLook playerMovementCamera;
    [SerializeField]
    private CinemachineVirtualCamera playerDialogueCamera;
    [SerializeField]
    private CinemachineFreeLook playerCombatCamera;

    [Header("Settings")]
    [SerializeField]
    private int highPriority = 11;
    [SerializeField]
    private int lowPriority = 0;
    
    private bool _onCombat = false;
    private float _maxSpeedX = 0;
    private float _maxSpeedY = 0;

    public void StopPlayerCameraMovement()
    {
        _maxSpeedX = playerMovementCamera.m_XAxis.m_MaxSpeed;
        _maxSpeedY = playerMovementCamera.m_YAxis.m_MaxSpeed;
        playerMovementCamera.m_XAxis.m_MaxSpeed = 0;
        playerMovementCamera.m_YAxis.m_MaxSpeed = 0;

    }

    public void RestorePlayerCameraMovement()
    {
        playerMovementCamera.m_XAxis.m_MaxSpeed = _maxSpeedX;
        playerMovementCamera.m_YAxis.m_MaxSpeed = _maxSpeedY;
    }

    private void Start()
    {
        InitalCameraSetup();
    }

    private void OnEnable()
    {
        DialogueManager.OnDialogueEnter += DialogueEnterCameras;
        DialogueManager.OnDialogueExit += DialogueExitCameras;
        PlayerAim.OnAimActive += AimActiveCameras;
        PlayerAim.OnAimInactive += AimInactiveCameras;
        PlayerClimb.OnClimbingEnter += ClimbingEnterCameras;
        PlayerClimb.OnClimbingExit += ClimbingExitCameras;
        EnemyManager.OnCombatEnter += CombatEnterCameras;
        EnemyManager.OnCombatExit += CombatExitCameras;
        SpawnManager.OnPlayerSpawn += PlayerSpawnCameraSetup;
        Player.OnPlayerDieTarget += PlayerDeathCameraSetup;
    }

    private void OnDisable()
    {
        DialogueManager.OnDialogueEnter -= DialogueEnterCameras;
        DialogueManager.OnDialogueExit -= DialogueExitCameras;
        PlayerAim.OnAimActive -= AimActiveCameras;
        PlayerAim.OnAimInactive -= AimInactiveCameras;
        PlayerClimb.OnClimbingEnter -= ClimbingEnterCameras;
        PlayerClimb.OnClimbingExit -= ClimbingExitCameras;
        EnemyManager.OnCombatEnter -= CombatEnterCameras;
        EnemyManager.OnCombatExit -= CombatExitCameras;
        SpawnManager.OnPlayerSpawn -= PlayerSpawnCameraSetup;
        Player.OnPlayerDieTarget -= PlayerDeathCameraSetup;
    }

    private void CombatEnterCameras()
    {
        _onCombat = true;
        playerCombatCamera.Priority = highPriority;
      
        playerAimCamera.Priority = lowPriority;
        playerClimbCamera.Priority = lowPriority;
        playerMovementCamera.Priority = lowPriority;
    }

    private void CombatExitCameras()
    {
        _onCombat = false;

        playerMovementCamera.Priority = highPriority;

        playerCombatCamera.Priority = lowPriority;
        playerClimbCamera.Priority = lowPriority;
        playerAimCamera.Priority = lowPriority;
    }

    private void DialogueEnterCameras()
    {
        playerDialogueCamera.Priority = highPriority;

        playerMovementCamera.Priority = lowPriority;
    }

    private void DialogueExitCameras()
    {
        playerMovementCamera.Priority = highPriority;

        playerDialogueCamera.Priority = lowPriority;
    }

    private void AimActiveCameras()
    {
        playerAimCamera.Priority = highPriority;

        playerClimbCamera.Priority = lowPriority;
        playerMovementCamera.Priority = lowPriority;
    }

    private void AimInactiveCameras()
    {
        playerAimCamera.Priority = lowPriority;
        if (!_onCombat)
        {
            playerMovementCamera.Priority = highPriority;
        }
          
    }

    private void ClimbingEnterCameras()
    {
        playerClimbCamera.Priority = highPriority;

        playerAimCamera.Priority = lowPriority;
        playerMovementCamera.Priority = lowPriority;
    }

    private void ClimbingExitCameras()
    {
        playerClimbCamera.Priority = lowPriority;
        if (!_onCombat)
        {
            playerMovementCamera.Priority = highPriority;
        }
    }

    private void PlayerDeathCameraSetup(GameObject target)
    {
        playerAimCamera.Priority = lowPriority;
        playerClimbCamera.Priority = lowPriority;

        playerMovementCamera.Priority = lowPriority;
        playerMovementCamera.Follow = target.transform;
        playerCombatCamera.Priority = lowPriority;
        playerCombatCamera.Follow = target.transform;

        playerDialogueCamera.Priority = lowPriority;
    }

    private void InitalCameraSetup()
    {
        playerAimCamera.Priority = lowPriority;
        playerClimbCamera.Priority = lowPriority;
        playerMovementCamera.Priority = highPriority;
        playerCombatCamera.Priority = lowPriority;
        playerDialogueCamera.Priority = lowPriority;
    }

    private void PlayerSpawnCameraSetup(GameObject playerObj) => InitalCameraSetup();
}
