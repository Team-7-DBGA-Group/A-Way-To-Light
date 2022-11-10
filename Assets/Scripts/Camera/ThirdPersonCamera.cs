using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField]
    private Transform Orientation;
    [SerializeField]
    private Transform Player;
    [SerializeField]
    private Transform PlayerObj;
    [SerializeField]
    private Rigidbody PlayerRB;


    [SerializeField]
    private PlayerMovement _playerMovement;

    private void Start()
    {
        _playerMovement = FindObjectOfType<PlayerMovement>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        Vector3 viewDir = Player.position - new Vector3(transform.position.x, Player.position.y, transform.position.z);
        Orientation.forward = viewDir.normalized;
        
        _playerMovement.ForwardPlayer.forward = Orientation.forward;
        _playerMovement.RotatePlayer();
        
    }
}
