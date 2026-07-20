using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 720f;
    [Header("Jump & Gravity")]
    [SerializeField] private float maxJumpHeight = 2f;
    [SerializeField] private float maxJumpTime = 0.5f;
    [SerializeField] private float groundedGravity = -2f;
    [Header("Animation")]
    [SerializeField] private float animationDampTime = 0.1f;

    private static readonly int MoveSpeedHash = Animator.StringToHash("moveSpeed");
    private static readonly int IsJumpingHash = Animator.StringToHash("isJumping");

    private Animator animator;
    private CharacterController controller;
    private Camera mainCamera;

    private Vector3 horizontalVelocity;
    private float verticalVelocity;
    private bool isRunPressed;
    private bool isJumping;
    private bool isJumpAnimating = false;
    private float gravity;
    private float initialJumpVelocity;

    private void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        mainCamera = Camera.main;
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
        if (Input.GetKeyDown(KeyCode.Alpha1)) Time.timeScale = 0.4f;
        if (Input.GetKeyDown(KeyCode.Alpha2)) Time.timeScale = 1f;

        HandleGravity();
        HandleJump();
        HandleMovement();
        if (controller.enabled)
            controller.Move((horizontalVelocity + Vector3.up * verticalVelocity) * Time.deltaTime);
        HandleAnimation();
    }

    private void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        isRunPressed = h != 0 || v != 0;
        if (!isRunPressed)
        {
            horizontalVelocity = Vector3.zero;
            return;
        }
        Vector3 camForward = Vector3.Scale(mainCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 camRight = Vector3.Scale(mainCamera.transform.right, new Vector3(1, 0, 1)).normalized;
        Vector3 targetVelocity = (camForward * v + camRight * h).normalized * moveSpeed;
        horizontalVelocity = targetVelocity;
        Quaternion targetRotation = Quaternion.LookRotation(horizontalVelocity);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    private void HandleGravity()
    {
        if (controller.isGrounded)
        {
            if (isJumpAnimating)
            {
                animator.SetBool(IsJumpingHash, false);
                isJumpAnimating = false;
            }
            verticalVelocity = groundedGravity;
        }
        else
            verticalVelocity += gravity * Time.deltaTime;
    }

    private void HandleJump()
    {
        if (!isJumping && controller.isGrounded && Input.GetButtonDown("Jump"))
        {
            isJumpAnimating = true;
            animator.SetBool(IsJumpingHash, true);
            isJumping = true;
            verticalVelocity = initialJumpVelocity;
        }

        if (isJumping && controller.isGrounded && verticalVelocity < 0f)
            isJumping = false;
    }

    private void HandleAnimation()
    {
        float currentSpeed = horizontalVelocity.magnitude;
        animator.SetFloat(MoveSpeedHash, currentSpeed, animationDampTime, Time.deltaTime);
    }
}