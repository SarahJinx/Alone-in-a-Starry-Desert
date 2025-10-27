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
    public float groundDistance = 0.3f;   // Radius of the sphere for ground detection
    public LayerMask groundMask;          // LayerMask for what counts as "Ground"

    private CharacterController controller;
    private Vector3 velocity;             // Vertical movement (gravity/jump)
    private bool isGrounded;              // Whether the player is touching the ground
    private bool jumpPressed;             // Prevents multiple jumps per button hold
    private bool isJumping;               // True while jump is active
    private float jumpTime;               // Time spent holding the jump

    // Jump settings
    private float maxJumpHoldTime = 0.30f;  // Max time to hold jump
    private float jumpBufferTime = 0.1f;    // Buffer window before landing
    private float jumpBufferCounter;       // Timer for buffered jump input

    void Start()
    {
        // Automatically get the CharacterController from the player
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // --- GROUND CHECK ---
        bool wasGrounded = isGrounded;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // --- JUMP BUFFER TIMER ---
        if (Input.GetButtonDown("Jump"))
        {
            // Start the buffer timer when jump is pressed
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            // Decrease buffer timer over time
            jumpBufferCounter -= Time.deltaTime;
        }

        // --- RESET ON LANDING ---
        if (isGrounded && !wasGrounded)
        {
            // Just landed
            isJumping = false;
            jumpTime = 0f;

            // If player pressed jump shortly before landing, jump immediately
            if (jumpBufferCounter > 0f)
            {
                PerformJump();
            }
        }

        // If on the ground and falling, reset vertical velocity
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // --- HORIZONTAL MOVEMENT (using Axis) ---
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Movement direction relative to the player
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Apply horizontal movement (XZ)
        controller.Move(move * speed * Time.deltaTime);

        // --- NORMAL JUMP INPUT ---
        if (Input.GetButtonDown("Jump") && isGrounded && !jumpPressed)
        {
            PerformJump();
        }

        // Reset jumpPressed once the button is released
        if (Input.GetButtonUp("Jump"))
            jumpPressed = false;

        // --- VARIABLE JUMP HEIGHT ---
        if (isJumping && Input.GetButton("Jump"))
        {
            jumpTime += Time.deltaTime;

            // While holding jump and within limit, reduce gravity effect
            if (jumpTime < maxJumpHoldTime)
            {
                velocity.y += (-gravity * 0.5f) * Time.deltaTime; // less gravity while holding
            }
        }

        // --- APPLY GRAVITY ---
        if (!isJumping || !Input.GetButton("Jump") || jumpTime >= maxJumpHoldTime)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        // Apply vertical movement (falling/jumping)
        controller.Move(velocity * Time.deltaTime);
    }

    // --- HANDLE JUMP EXECUTION ---
    private void PerformJump()
    {
        velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        isJumping = true;
        jumpPressed = true;
        jumpTime = 0f;
        jumpBufferCounter = 0f; // reset buffer after performing jump
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a sphere for ground detection
        Gizmos.color = isGrounded ? Color.green : Color.red;
        if (groundCheck != null)
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }
}
