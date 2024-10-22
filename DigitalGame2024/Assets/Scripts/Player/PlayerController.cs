using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Variables for movement and camera control
    public float moveSpeed = 5f; // Speed at which the player moves
    public float lookSpeed = 2f; // Speed of the camera rotation (looking around)
    public Camera playerCamera; // Reference to the player's camera

    private float yaw; // Yaw (horizontal rotation) for camera control
    private float pitch; // Pitch (vertical rotation) for camera control
    private Rigidbody rb; // Reference to the player's Rigidbody component for physics-based movement

    private void Awake()
    {
        // Initialize the Rigidbody component
        rb = GetComponent<Rigidbody>();
        
        // Lock the cursor in the center of the screen and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Initialize yaw and pitch with the current rotation values of the player and camera
        yaw = transform.eulerAngles.y;
        pitch = playerCamera.transform.localEulerAngles.x;
    }

    private void Update()
    {
        // Handle camera rotation based on mouse input
        RotateCamera();
    }

    private void FixedUpdate()
    {
        // Handle player movement in FixedUpdate for consistent physics updates
        Move();
    }

    private void Move()
    {
        // Get input from the horizontal (A/D or left/right arrow) and vertical (W/S or up/down arrow) axes
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Create a movement vector based on input
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        // Transform the movement vector from local space to world space, aligning it with the player's direction
        movement = transform.TransformDirection(movement);
        
        // Move the player by updating the Rigidbody's position based on the movement vector, speed, and time
        rb.MovePosition(transform.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void RotateCamera()
    {
        // Get mouse input for camera rotation (Mouse X for yaw, Mouse Y for pitch)
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        // Adjust yaw (horizontal rotation) and pitch (vertical rotation)
        yaw += mouseX;
        pitch -= mouseY;
        // Clamp the pitch value to prevent the camera from rotating beyond vertical limits (e.g., upside down)
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        // Apply the pitch to the camera's local rotation (up/down view)
        playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        // Apply the yaw to the player's rotation (left/right view)
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
    }
}
