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
        DialogueManager.OnDialogueEnter += () => 
        {
            playerDialogueCamera.SetActive(true); 
            playerMovementCamera.SetActive(false); 
        };
        DialogueManager.OnDialogueExit += () => 
        {
            playerDialogueCamera.SetActive(false); 
            playerMovementCamera.SetActive(true); 
        };
        PlayerAim.OnAimActive += () =>
        {
            playerAimCamera.SetActive(true);
            playerClimbCamera.SetActive(false);
            playerMovementCamera.SetActive(false);
        };
        PlayerAim.OnAimInactive += () =>
        {
            playerAimCamera.SetActive(false);
            playerMovementCamera.SetActive(true);
        };
        PlayerClimb.OnClimbingEnter += () =>
        {
             playerAimCamera.SetActive(false);
             playerClimbCamera.SetActive(true);
             playerMovementCamera.SetActive(false);
        };
        PlayerClimb.OnClimbingExit += () =>
        {
            playerClimbCamera.SetActive(false);
            playerMovementCamera.SetActive(true);
        };
    }
}
