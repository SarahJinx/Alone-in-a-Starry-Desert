using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;              // Movement speed
    public float jumpForce = 8f;          // Jump strength
    public float gravity = -9.81f;        // Gravity value

    [Header("Ground Check")]
    public Transform groundCheck;         // Point below the player
    public float groundDistance = 0.3f;   // Radius for ground detection
    public LayerMask groundMask;          // LayerMask for what counts as "Ground"

    [Header("Camera")]
    public Transform cam;                 // Reference to camera

    [Header("Rotation")]
    public float rotationSmoothTime = 0.1f; // Smooth rotation time
    private float rotationVelocity;

    private CharacterController controller;
    private Vector3 velocity;             // Vertical movement (gravity/jump)
    private bool isGrounded;              // Whether touching ground
    private bool jumpPressed;             // Prevent multiple jumps per hold
    private bool isJumping;               // True while jump is active
    private float jumpTime;               // Time holding jump

    // Jump settings
    private float maxJumpHoldTime = 0.30f;  // Max hold time for jump
    private float jumpBufferTime = 0.1f;    // Buffer window before landing
    private float jumpBufferCounter;        // Timer for buffered jump input

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Automatically assign main camera if cam not set
        if (cam == null && Camera.main != null)
            cam = Camera.main.transform;
    }

    void Update()
    {
        // --- GROUND CHECK ---
        bool wasGrounded = isGrounded;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // --- JUMP BUFFER TIMER ---
        if (Input.GetButtonDown("Jump"))
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;

        // --- RESET ON LANDING ---
        if (isGrounded && !wasGrounded)
        {
            isJumping = false;
            jumpTime = 0f;

            // Jump immediately if buffered
            if (jumpBufferCounter > 0f)
                PerformJump();
        }

        // Reset vertical velocity when grounded
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // --- HORIZONTAL MOVEMENT BASED ON CAMERA ---
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Vector de entrada normalizado
        Vector3 inputVector = new Vector3(horizontal, 0f, vertical).normalized;

        // Si hay cámara, proyectar su dirección al plano horizontal
        Vector3 move = Vector3.zero;
        if (cam != null)
        {
            Vector3 camForward = cam.forward;
            Vector3 camRight = cam.right;

            // Eliminar la inclinación vertical de la cámara (importante)
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            move = camForward * vertical + camRight * horizontal;
        }
        else
        {
            move = inputVector;
        }

        if (cam != null)
        {
            // Get camera yaw only
            float cameraYaw = cam.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, cameraYaw, 0f);

            // Rotate input by camera yaw
            move = rotation * inputVector;
        }

        // Apply horizontal movement
        Debug.Log($"Move vector: {move}, magnitude: {move.magnitude}");
        controller.Move(move * speed * Time.deltaTime);

        // Rotate player smoothly toward move direction if there's input
        if (inputVector.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        // --- NORMAL JUMP INPUT ---
        if (Input.GetButtonDown("Jump") && isGrounded && !jumpPressed)
            PerformJump();

        if (Input.GetButtonUp("Jump"))
            jumpPressed = false;

        // --- VARIABLE JUMP HEIGHT ---
        if (isJumping && Input.GetButton("Jump"))
        {
            jumpTime += Time.deltaTime;
            if (jumpTime < maxJumpHoldTime)
                velocity.y += (-gravity * 0.5f) * Time.deltaTime; // reduced gravity while holding
        }

        // --- APPLY GRAVITY ---
        if (!isJumping || !Input.GetButton("Jump") || jumpTime >= maxJumpHoldTime)
            velocity.y += gravity * Time.deltaTime;

        // Apply vertical movement
        controller.Move(velocity * Time.deltaTime);
    }

    // --- HANDLE JUMP ---
    private void PerformJump()
    {
        velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        isJumping = true;
        jumpPressed = true;
        jumpTime = 0f;
        jumpBufferCounter = 0f; // reset buffer
    }

    private void OnDrawGizmosSelected()
    {
        // Draw ground detection sphere
        Gizmos.color = isGrounded ? Color.green : Color.red;
        if (groundCheck != null)
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }
}
