using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    private float PlayerSpeed = 5f;
    [SerializeField]
    private float PlayerRunMultiplier = 1.25f;
    [SerializeField]
    private float RotationSpeed = 5f;

    public Transform ForwardPlayer { get; set; }

    private float _horizontalInput;
    private float _verticalInput;
    private float _ySpeed;
    private Rigidbody _playerRB;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerRB = GetComponent<Rigidbody>();
        ForwardPlayer = transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        _ySpeed += Physics.gravity.y * Time.deltaTime;
        if (!Input.anyKey)
        {
            _playerRB.velocity = Vector3.zero;
        }
        
        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.LeftShift))
                _playerRB.velocity = ForwardPlayer.forward * PlayerSpeed * PlayerRunMultiplier;
            else
                _playerRB.velocity = ForwardPlayer.forward * PlayerSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            _playerRB.velocity = -ForwardPlayer.right * PlayerSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            _playerRB.velocity = -ForwardPlayer.forward * PlayerSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _playerRB.velocity = ForwardPlayer.right * PlayerSpeed;
        }
        
    }

    public void RotatePlayer()
    {
        Vector3 inputDir = ForwardPlayer.forward * _verticalInput + ForwardPlayer.right * _horizontalInput;
        if (inputDir != Vector3.zero)
            ForwardPlayer.forward = Vector3.Slerp(ForwardPlayer.forward, inputDir, Time.deltaTime * RotationSpeed);
    }
}
