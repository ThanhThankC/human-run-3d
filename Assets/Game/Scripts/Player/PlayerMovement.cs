using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 720f;
    [Header("Jump")]
    [SerializeField] private float maxJumpHeight = 2f;
    [SerializeField] private float maxJumpTime = 0.5f;
    [SerializeField] private int maxJumps = 2;
    [Header("Gravity")]
    [SerializeField] private float groundedGravity = -2f;
    [Header("Animation")]
    [SerializeField] private float animationDampTime = 0.1f;
    [Header("Joystick")]
    [SerializeField] private FloatingJoystick joystick;

    private static readonly int MoveSpeedHash = Animator.StringToHash("moveSpeed");
    private static readonly int IsJumpingHash = Animator.StringToHash("isJumping");
    private static readonly int IsDoubleJumpingHash = Animator.StringToHash("isDoubleJumping");

    private Animator animator;
    private CharacterController controller;
    private Camera mainCamera;

    private float initialJumpVelocity;
    private Vector3 horizontalVelocity;
    private float verticalVelocity;
    private float gravity;

    private bool isRunPressed;
    private int jumpsRemaining;
    private bool isJumpAnimating;
    private bool isJumpButtonPressed;

    private void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        mainCamera = Camera.main;
        jumpsRemaining = maxJumps;
        SetupJumpVariables();
    }

    private void SetupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2f;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = -gravity * timeToApex;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha1)) Time.timeScale = 0.4f;
        if (Input.GetKeyDown(KeyCode.Alpha2)) Time.timeScale = 1f;
#endif

        HandleGravity();
        HandleJump();
        HandleMovement();
        if (controller.enabled)
            controller.Move((horizontalVelocity + Vector3.up * verticalVelocity) * Time.deltaTime);
        HandleAnimation();
    }

    public void PressedJumpButton() => isJumpButtonPressed = true;

    private void HandleMovement()
    {
        Vector2 input = GetMoveInput();
        isRunPressed = input.sqrMagnitude > 0.01f;

        if (!isRunPressed)
        {
            horizontalVelocity = Vector3.zero;
            return;
        }
        Vector3 camForward = Vector3.Scale(mainCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 camRight = Vector3.Scale(mainCamera.transform.right, new Vector3(1, 0, 1)).normalized;
        Vector3 targetVelocity = (camForward * input.y + camRight * input.x).normalized * moveSpeed;
        horizontalVelocity = targetVelocity;
        Quaternion targetRotation = Quaternion.LookRotation(horizontalVelocity);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    private Vector2 GetMoveInput()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
#else
        return joystick != null ? new Vector2(joystick.Horizontal, joystick.Vertical) : Vector2.zero;
#endif
    }

    private void HandleGravity()
    {
        if (controller.isGrounded)
        {
            if (isJumpAnimating)
            {
                animator.SetBool(IsJumpingHash, false);
                animator.SetBool(IsDoubleJumpingHash, false);
                isJumpAnimating = false;
            }
            jumpsRemaining = maxJumps;
            verticalVelocity = groundedGravity;
        }
        else
            verticalVelocity += gravity * Time.deltaTime;
    }

    private void HandleJump()
    {
        bool jumpPressed = Input.GetButtonDown("Jump") || isJumpButtonPressed;
        isJumpButtonPressed = false;

        if (jumpsRemaining > 0 && jumpPressed)
        {
            bool isDoubleJump = jumpsRemaining < maxJumps;
            isJumpAnimating = true;
            animator.SetBool(IsJumpingHash, true);
            verticalVelocity = initialJumpVelocity;
            jumpsRemaining--;
            if (isDoubleJump)
            {
                animator.SetBool(IsJumpingHash, false);
                animator.SetBool(IsDoubleJumpingHash, true);
            }
            else
            {
                animator.SetBool(IsJumpingHash, true);
                animator.SetBool(IsDoubleJumpingHash, false);
            }
        }
    }

    private void HandleAnimation()
    {
        float currentSpeed = horizontalVelocity.magnitude;
        animator.SetFloat(MoveSpeedHash, currentSpeed, animationDampTime, Time.deltaTime);
    }
}