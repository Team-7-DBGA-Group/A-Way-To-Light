using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool IsGrounded { get; private set; }
    public bool CanMove { get; set; }

    [Header("References")]
    [SerializeField]
    private Player player;
    [SerializeField]
    private PlayerAim playerAim;
    [SerializeField]
    private PlayerClimb playerClimb;

    [Header("Movement settings")]
    [SerializeField]
    private float playerSpeed = 5f;
    [SerializeField]
    private float playerRunMultiplier = 1.25f;
    [SerializeField]
    private float turnSmoothTime = 0.1f;
    [SerializeField]
    private Transform mainCameraTransform = null;

    [Header("Jump settings")]
    [SerializeField]
    private Transform groundCheck = null;
    [SerializeField]
    private LayerMask groundMask = new LayerMask();
    [SerializeField]
    private LayerMask climbGroundMask = new LayerMask();
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float groundDistance = 0.2f;
    [SerializeField]
    private float jumpHeight = 2.0f;

    private CharacterController _characterController;

    private float _horizontalInput;
    private float _verticalInput;
    private float _turnSmoothVelocity;

    private Vector3 _velocity;

    private void Start()
    {
        CanMove = true;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _characterController = GetComponent<CharacterController>();
        float correctHeight = _characterController.center.y + _characterController.skinWidth;
        _characterController.center = new Vector3(0, correctHeight, 0);
    }

    private void OnEnable()
    {
        DialogueManager.OnDialogueEnter += () => { CanMove = false; _characterController.Move(Vector3.zero); };
        DialogueManager.OnDialogueExit += () => { CanMove = true; };
        player.OnKnockbackEnter += () => { CanMove = false; };
        player.OnKnockbackExit += () => { CanMove = true; };
    }

    private void Update()
    {
        if (!CanMove)
            return;

        IsGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (!IsGrounded)
            IsGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, climbGroundMask);

        if (IsGrounded)
            playerClimb.WasClimbing = false;

        if (IsGrounded && _velocity.y < 0)
            _velocity.y = -2f;

        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(_horizontalInput, 0f, _verticalInput).normalized;

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
            _velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);

        if (!playerClimb.IsClimbing)
        {
            _velocity.y += gravityValue * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);
        }

        if (direction.magnitude >= 0.1 && !playerAim.IsAiming && !playerClimb.IsClimbing)
        {
            // Calcolo rotazione player tenendo conto di dove sta guardando la camera
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Calcolo direzione player
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Movimento del Player
            if (Input.GetKey(KeyCode.LeftShift))
                _characterController.Move(moveDir.normalized * playerSpeed * Time.deltaTime * playerRunMultiplier);
            else
                _characterController.Move(moveDir.normalized * playerSpeed * Time.deltaTime);

        }
    }
}
