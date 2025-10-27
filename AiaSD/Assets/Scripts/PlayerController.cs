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

    void Start()
    {
        // Automatically get the CharacterController from the player
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // --- GROUND CHECK ---
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

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

        // --- JUMP ---
        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);

        // --- GRAVITY ---
        velocity.y += gravity * Time.deltaTime;

        // Apply vertical movement (falling/jumping)
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a sphere for ground detection
        Gizmos.color = isGrounded ? Color.green : Color.red;
        if (groundCheck != null)
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }
}
