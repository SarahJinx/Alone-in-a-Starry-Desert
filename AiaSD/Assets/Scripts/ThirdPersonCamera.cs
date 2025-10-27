using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform target;              // The player to follow
    public float distance = 3f;           // Distance from the player
    public float sensitivity = 2f;        // Mouse sensitivity
    public float verticalLimit = 80f;     // Limit vertical rotation

    private float rotationX = 0f;         // Vertical rotation (pitch)
    private float rotationY = 0f;         // Horizontal rotation (yaw)

    void Start()
    {
        // Lock and hide the cursor for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // Do not move camera if the game is paused
        if (PauseMenu.GameIsPaused)
            return;

        if (!target) return;

        // --- MOUSE INPUT ---
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // Update rotation values
        rotationY += mouseX;
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -verticalLimit, verticalLimit);

        // --- ROTATION AROUND PLAYER ---
        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
        Vector3 direction = new Vector3(0, 0, -distance);

        // Position camera behind the player
        transform.position = target.position + rotation * direction;

        // Look at the player
        transform.LookAt(target.position);
    }
}
