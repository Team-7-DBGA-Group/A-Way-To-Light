using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraAutoTarget : MonoBehaviour
{
    public enum TargetType
    {
        PlayerTarget,
        DialogueTarget
    }

    [Header("References")]
    [SerializeField]
    private CinemachineVirtualCameraBase targetCamera;

    [Header("Settings")]
    [SerializeField]
    private TargetType targetType = TargetType.PlayerTarget;

    public void SetCameraFollow(GameObject playerObj)
    {
        targetCamera.Follow = playerObj.transform;
    }

    public void SetCameraLookAt(GameObject playerObj)
    {
        switch (targetType)
        {
            case TargetType.PlayerTarget:
                {
                    targetCamera.LookAt = playerObj.GetComponentInChildren<PlayerCameraTarget>().transform;
                    break;
                }
            case TargetType.DialogueTarget:
                {
                    targetCamera.LookAt = playerObj.GetComponentInChildren<DialogueCameraTarget>().transform;
                    break;
                }
        }
    }

    private void OnEnable()
    {
        SpawnManager.OnPlayerSpawn += SetCameraFollow;
        SpawnManager.OnPlayerSpawn += SetCameraLookAt;
    }

    private void OnDisable()
    {
        SpawnManager.OnPlayerSpawn -= SetCameraFollow;
        SpawnManager.OnPlayerSpawn -= SetCameraLookAt;
    }
}
