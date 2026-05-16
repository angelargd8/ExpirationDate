using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float sprintSpeed = 4f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.25f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Camera")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float cameraDistance = 1f;
    [SerializeField] private float cameraHeight = 0.4f;
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float minPitch = -30f;
    [SerializeField] private float maxPitch = 65f;

    private Rigidbody rb;
    private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction sprintAction;

    private Vector2 moveInput;
    private Vector2 lookInput;

    private bool isGrounded;
    private bool isSprinting;

    private float yaw;
    private float pitch = 20f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];
        jumpAction = playerInput.actions["Jump"];
        sprintAction = playerInput.actions["Sprint"];
    }

    private void OnEnable()
    {
        moveAction.performed += OnMovePerformed;
        moveAction.canceled += OnMoveCanceled;

        lookAction.performed += OnLookPerformed;
        lookAction.canceled += OnLookCanceled;

        jumpAction.performed += OnJumpPerformed;

        sprintAction.performed += OnSprintPerformed;
        sprintAction.canceled += OnSprintCanceled;
    }

    private void OnDisable()
    {
        moveAction.performed -= OnMovePerformed;
        moveAction.canceled -= OnMoveCanceled;

        lookAction.performed -= OnLookPerformed;
        lookAction.canceled -= OnLookCanceled;

        jumpAction.performed -= OnJumpPerformed;

        sprintAction.performed -= OnSprintPerformed;
        sprintAction.canceled -= OnSprintCanceled;

        moveInput = Vector2.zero;
        lookInput = Vector2.zero;
        isSprinting = false;
    }

    private void Update()
    {
        CheckGround();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void LateUpdate()
    {
        RotateCamera();
    }

    private void MovePlayer()
    {
        if (cameraTransform == null) return;

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 direction = cameraForward * moveInput.y + cameraRight * moveInput.x;

        if (direction.magnitude > 1f)
        {
            direction.Normalize();
        }

        float currentSpeed = isSprinting ? sprintSpeed : speed;

        Vector3 movement = direction * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                12f * Time.fixedDeltaTime
            );
        }
    }

    private void RotateCamera()
    {
        if (cameraTransform == null || cameraTarget == null) return;

        yaw += lookInput.x * mouseSensitivity * Time.deltaTime;
        pitch -= lookInput.y * mouseSensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion cameraRotation = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 targetPosition = cameraTarget.position + Vector3.up * cameraHeight;
        Vector3 cameraPosition = targetPosition - cameraRotation * Vector3.forward * cameraDistance;

        cameraTransform.position = cameraPosition;
        cameraTransform.rotation = cameraRotation;
    }

    private void CheckGround()
    {
        if (groundCheck == null) return;

        isGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    private void OnLookCanceled(InputAction.CallbackContext context)
    {
        lookInput = Vector2.zero;
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (!isGrounded) return;

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void OnSprintPerformed(InputAction.CallbackContext context)
    {
        isSprinting = true;
    }

    private void OnSprintCanceled(InputAction.CallbackContext context)
    {
        isSprinting = false;
    }
}