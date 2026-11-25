using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float playerSpeed = 5f;
    public float acceleration = 25f;
    public float deceleration = 12f;

    [Header("Jump / Gravity")]
    public float jumpHeight = 1.5f;
    public float gravityValue = -9.81f; // Earth gravity

    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference jumpAction;

    [Header("References")]
    public Transform cameraTransform;

    [Header("Animation")]
    public Animator animator;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private Vector3 currentHorizontalVelocity;
    private bool groundedPlayer;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }
    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
    }
    private void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
    }

    private void Update()
    {
        groundedPlayer = controller.isGrounded;

        // In order for the game to know what x and z are in 3D based on the camera's placement, we need to
        // flatten it to the ground, we can do this with .ProjectOnPlane
        Vector3 camForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
        Vector3 camRight = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;

        Vector2 input = moveAction.action.ReadValue<Vector2>();
        Vector3 move = camForward * input.y + camRight * input.x;   // multiplying by .y and .x gives us negatives and positives for movement
        move = Vector3.ClampMagnitude(move, 1f);

        Vector3 desiredVelocity = move * playerSpeed;

        // Accelerate or decelerate
        if (move.magnitude > 0.1f)
        {
            currentHorizontalVelocity = Vector3.MoveTowards(currentHorizontalVelocity, desiredVelocity, acceleration * Time.deltaTime);
        }
        else
            currentHorizontalVelocity = Vector3.MoveTowards(currentHorizontalVelocity, Vector3.zero, deceleration * Time.deltaTime);

        // Jump + gravity
        if (groundedPlayer && playerVelocity.y < 0f)
        {
            playerVelocity.y = -2f; // fixes a jump not casting bug
        }

        if (jumpAction.action.WasPressedThisFrame() && groundedPlayer)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue); // Jump formula taken from Unity Documentation
        }

        playerVelocity.y += gravityValue * Time.deltaTime;

        Vector3 finalMove = currentHorizontalVelocity + Vector3.up * playerVelocity.y;
        controller.Move(finalMove * Time.deltaTime);

        // this updates the animator's speed float to match the player's movement so it knows
        // when to walk versus stop walking.
        animator.SetFloat("Speed", currentHorizontalVelocity.magnitude);
        animator.SetBool("Grounded", groundedPlayer);
        animator.SetFloat("VerticalSpeed", playerVelocity.y);
    }
}