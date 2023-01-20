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

    private CinemachineVirtualCameraBase _currentCamera = null;

    public void StopPlayerCameraMovement()
    {
        if(_currentCamera is CinemachineFreeLook)
        {
            CinemachineFreeLook flCamera = (CinemachineFreeLook)_currentCamera;
            _maxSpeedX = flCamera.m_XAxis.m_MaxSpeed;
            _maxSpeedY = flCamera.m_YAxis.m_MaxSpeed;
            flCamera.m_XAxis.m_MaxSpeed = 0;
            flCamera.m_YAxis.m_MaxSpeed = 0;
        }
    }

    public void RestorePlayerCameraMovement()
    {
        if (_currentCamera is CinemachineFreeLook)
        {
            CinemachineFreeLook flCamera = (CinemachineFreeLook)_currentCamera;
            flCamera.m_XAxis.m_MaxSpeed = _maxSpeedX;
            flCamera.m_YAxis.m_MaxSpeed = _maxSpeedY;
        }
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
        GameManager.OnPause += HandlePause;
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
        GameManager.OnPause -= HandlePause;
    }

    private void CombatEnterCameras()
    {
        _onCombat = true;
        playerCombatCamera.Priority = highPriority;
        _currentCamera = playerCombatCamera;
      
        playerAimCamera.Priority = lowPriority;
        playerClimbCamera.Priority = lowPriority;
        playerMovementCamera.Priority = lowPriority;
    }

    private void CombatExitCameras()
    {
        _onCombat = false;

        playerMovementCamera.Priority = highPriority;
        _currentCamera = playerMovementCamera;

        playerCombatCamera.Priority = lowPriority;
        playerClimbCamera.Priority = lowPriority;
        playerAimCamera.Priority = lowPriority;
    }

    private void DialogueEnterCameras()
    {
        playerDialogueCamera.Priority = highPriority;
        _currentCamera = playerDialogueCamera;

        playerMovementCamera.Priority = lowPriority;
    }

    private void DialogueExitCameras()
    {
        playerMovementCamera.Priority = highPriority;
        _currentCamera = playerMovementCamera;

        playerDialogueCamera.Priority = lowPriority;
    }

    private void AimActiveCameras()
    {
        playerAimCamera.Priority = highPriority;
        _currentCamera = playerAimCamera;

        playerClimbCamera.Priority = lowPriority;
        playerMovementCamera.Priority = lowPriority;
    }

    private void AimInactiveCameras()
    {
        playerAimCamera.Priority = lowPriority;
        if (!_onCombat)
        {
            playerMovementCamera.Priority = highPriority;
            _currentCamera = playerMovementCamera;
        }
    }

    private void ClimbingEnterCameras()
    {
        playerClimbCamera.Priority = highPriority;
        _currentCamera = playerClimbCamera;

        playerAimCamera.Priority = lowPriority;
        playerMovementCamera.Priority = lowPriority;
    }

    private void ClimbingExitCameras()
    {
        playerClimbCamera.Priority = lowPriority;
        if (!_onCombat)
        {
            playerMovementCamera.Priority = highPriority;
            _currentCamera = playerMovementCamera;
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

        _currentCamera = null;
    }

    private void InitalCameraSetup()
    {
        playerAimCamera.Priority = lowPriority;
        playerClimbCamera.Priority = lowPriority;
        
        playerMovementCamera.Priority = highPriority;
        _currentCamera = playerMovementCamera;

        playerCombatCamera.Priority = lowPriority;
        playerDialogueCamera.Priority = lowPriority;
    }

    private void PlayerSpawnCameraSetup(GameObject playerObj) => InitalCameraSetup();

    private void HandlePause(bool isPause)
    {
        if (isPause)
        {
            StopPlayerCameraMovement();
        }
        else
        {
            RestorePlayerCameraMovement();
        }
    }
}
