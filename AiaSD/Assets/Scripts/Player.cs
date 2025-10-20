using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float walkSpeed = 7f;
    public float runSpeed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 5f;
    public float jumps = 1;
    public float groundDistance = 0.4f;
    public float mouseSens = 0.5f; //cant be implemented with time*deltaTime
    public Transform playerCamera;
    public LayerMask groundMask = 1;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private Transform groundCheck;

    private float lookRotation = 0f;
    private float mouseX;
    private float mouseY;

    private Vector3 move;
    private float x;
    private float y;
    private float z;
    private bool isRunning;
    private float currSpeed;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        CreateGroundCheck();

        Cursor.lockState = CursorLockMode.Locked;

        playerCamera = Camera.main?.transform;
        if (playerCamera == null)
        {
            Debug.LogError("No camera assigned and no main camera found!");
        }
    }
    void Update()
    {
        HandleGroundCheck();
        HandleMouseLook();
        HandleMovement();
        HandleJump();
        HandleGravity();

        controller.Move (move * Time.deltaTime);
    }

    void CreateGroundCheck()
    {
        GameObject groundCheckObj = new GameObject("GroundCheck");
        groundCheckObj.transform.SetParent(transform);
        groundCheckObj.transform.localPosition = new Vector3(0, -controller.height / 2f, 0);
        groundCheck = groundCheckObj.transform;
    }

    void HandleGroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y > 0)
        {
            velocity.y = -2F;
        }
    }

    void HandleMouseLook()
    {
        if (playerCamera == null) return;

        // Get mouse input
        mouseX = Input.GetAxis("Mouse X") * mouseSens;
        mouseY = Input.GetAxis("Mouse Y") * mouseSens;

        // Rotate cam up/down
        lookRotation -= mouseY; // Get MouseY input and translate it inverted to lookRotation
        lookRotation = Mathf.Clamp(lookRotation, -90f, 90f);    // Clamp it
        playerCamera.localRotation = Quaternion.Euler(lookRotation, 0f, 0f); // Rotate the camera vertically based on lookRotations input

        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        // Get input
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        // Check if running
        isRunning = Input.GetKey(KeyCode.LeftShift);
        currSpeed = isRunning ? runSpeed : walkSpeed;

        // Calculate movement direction relative to player
        move = transform.right * x + transform.forward * z;
        move = Vector3.ClampMagnitude(move, 1f) * currSpeed;

        // Add vertical velocity
        move.y = velocity.y;
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void HandleGravity()
    {
        velocity.y += gravity * Time.deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawSphere(groundCheck.position, groundDistance);
        }
    }

    public bool IsGrounded => isGrounded;
    public bool IsRunning => isRunning;
    public float CurrentSpeed => currSpeed;
    public Vector3 Velocity => controller.velocity;
    
    public void ToggleCursorLock()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
