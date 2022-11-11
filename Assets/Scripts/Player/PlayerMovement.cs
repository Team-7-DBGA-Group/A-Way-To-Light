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
    private float TurnSmoothTime = 0.1f;
    [SerializeField]
    private Transform CameraTransform = null;

    [Header("Jump settings")]
    [SerializeField]
    private Transform groundCheck = null;
    [SerializeField]
    private LayerMask groundMask = new LayerMask();
    [SerializeField]
    private float _gravityValue = -9.81f;
    [SerializeField]
    private float _groundDistance = 0.2f;

    private CharacterController _characterController;

    private float _horizontalInput;
    private float _verticalInput;
    private float _turnSmoothVelocity;

    private Vector3 _velocity;
    private bool _isGrounded;
    private float jumpHeight = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _characterController = GetComponent<CharacterController>();
        float correctHeight = _characterController.center.y + _characterController.skinWidth;
        _characterController.center = new Vector3(0, correctHeight, 0);
    }

    // Update is called once per frame
    void Update()
    {
        _isGrounded = Physics.CheckSphere(groundCheck.position, _groundDistance, groundMask);

        if (_isGrounded && _velocity.y < 0)
            _velocity.y = -2f;

        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(_horizontalInput, 0f, _verticalInput).normalized;

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
            _velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * _gravityValue);

        _velocity.y += _gravityValue * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);

        if (direction.magnitude >= 0.1)
        {

            //Calcolo rotazione player tenendo conto di dove sta guardando la camera
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + CameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, TurnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //Calcolo direzione player
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            //Movimento del Player
            if (Input.GetKey(KeyCode.LeftShift))
                _characterController.Move(moveDir.normalized * PlayerSpeed * Time.deltaTime * PlayerRunMultiplier);
            else
                _characterController.Move(moveDir.normalized * PlayerSpeed * Time.deltaTime);

        }

        
    }
}
