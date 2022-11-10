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

    private float _horizontalInput;
    private float _verticalInput;
    private CharacterController _characterController;
    private float _turnSmoothVelocity;

    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(_horizontalInput, 0f, _verticalInput).normalized;

        if(direction.magnitude >= 0.1)
        {
            //Calcolo rotazione player tenendo conto di dove sta guardando la camera
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + CameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, TurnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //Calcolo direzione player
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            //Movimento del Player
            //_characterController.Move(direction * angle * Time.deltaTime);

            _characterController.Move(moveDir.normalized * PlayerSpeed * Time.deltaTime);
        }
    }
}
