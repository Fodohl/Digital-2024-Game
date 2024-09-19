using System.Collections;
using TMPro;
using UnityEngine;
using Alteruna;
using System;
using System.Collections.Generic;
using JetBrains.Annotations;

public class OriginalFirstPersonMovement : CommunicationBridge
{
    // Rigidbody that controls the player
    private Rigidbody rb;
    // The main camera of the player
    [SerializeField] private Camera cam;
    // Visible player mesh
    [SerializeField] private GameObject[] playerMesh;
    // The position at which the ground check is performed
    [SerializeField] private Transform groundCheck;
    // The height of the ground check
    [SerializeField] private float groundCheckRange;
    // A layermask that doesn't include the player
    [SerializeField] private LayerMask notPlayerMask;
    // Look sensitivity
    [SerializeField] private float LookSens = 3f;
    // Player speed
    [SerializeField] private float speed = 8000f;
    // Player jump height
    [SerializeField] private float jumpHeight = 10f;
    // Key codes set in inspector
    [SerializeField] private KeyCode jumpKey, crouchKey;
    // The grounded state of the player
    public bool isGrounded;
    // For logic behind jumping
    public bool canJump;
    // Bool to determine whether the player is sliding
    public bool isSliding;
    // Input variables
    private float xInput, yInput, xMouseInput, yMouseInput;
    // A bool that tells us if the player dies so that we can run different logic
    private bool dead = false;
    // Alteruna avatar
    private Alteruna.Avatar avatar;
    private Vector3 slopeMoveDirection;
    [SerializeField] private float slopeCounter;
    private bool setUp = false;
    public bool isSwimming = false;
    [SerializeField] private float waterHeight = 4.5f;
    private GameObject[] indicators;

    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    private const string MOUSE_X = "Mouse X";
    private const string MOUSE_Y = "Mouse Y";

    // Basic player setup (cursor and setting rigidbody)
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        avatar = GetComponent<Alteruna.Avatar>();
        if (avatar.IsMe)
        {
            indicators = GameObject.FindGameObjectsWithTag("PlayerIndicator");
        }
    }

    private void Update()
    {
        // Setup for the player and camera
        if (!setUp)
        {
            if (!avatar.IsMe)
            {
                GetComponentInChildren<AudioListener>().enabled = false; // Disable audio listener for non-local player
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked; // Lock cursor for local player
                Cursor.visible = false; // Hide cursor
                foreach (var item in playerMesh)
                {
                    item.SetActive(false); // Hide player mesh for local player
                }
                setUp = true; // Mark setup as complete
            }
        }
        
        // Check if player is in water
        if (transform.position.y < waterHeight)
        {
            EndSlide(); // End slide if in water
            isSwimming = true; // Set swimming state
            rb.useGravity = false; // Disable gravity while swimming
            rb.drag = 0.5f; // Apply drag in water
        }
        else
        {
            isSwimming = false; // Reset swimming state
            rb.useGravity = true; // Re-enable gravity
            rb.drag = 0f; // Reset drag
        }

        // Handle movement and look based on player state
        if (avatar.IsMe && !isSwimming)
        {
            Movement(); // Handle regular movement
            Look(); // Handle camera rotation
            PlayerInput(); // Process player input
        }
        else if (avatar.IsMe && isSwimming)
        {
            SwimMovement(); // Handle swimming movement
            Look(); // Handle camera rotation
            PlayerInput(); // Process player input
        }

        GroundCheck(); // Perform ground check

        // Update player indicators for local player
        if (avatar.IsMe)
        {
            indicators = GameObject.FindGameObjectsWithTag("PlayerIndicator");
            foreach (var dot in indicators)
            {
                // Adjust position and scale of indicators based on distance
                dot.transform.localPosition = new Vector3(dot.transform.localPosition.x, Vector3.Distance(transform.position, dot.transform.position) * 0.5f, dot.transform.localPosition.z);
                dot.transform.localScale = Vector3.one * Vector3.Distance(transform.position, dot.transform.position) * 0.25f;
            }
        }
    }

    RaycastHit slopeHit;
    
    // Detects if the player is on a slope and returns a boolean value
    private bool SlopeDetection()
    {
        // Perform a raycast downwards to check for slope
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, 2))
        {
            // Check if the slope's normal is not pointing straight up
            if (slopeHit.normal != Vector3.up)
            {
                return true; // Slope detected
            }
            else
            {
                return false; // No slope detected
            }
        }
        return false; // No slope detected
    }

    // Manages player input for movement and actions based on the game state
    private void PlayerInput()
    {
        // Ensure player is in a playable state
        if (GameManager.Instance.gameState == GameManager.GameState.Playing || GameManager.Instance.gameState == GameManager.GameState.ScoreBoard)
        {
            // Get input for movement and mouse
            xInput = Input.GetAxisRaw(HORIZONTAL);
            yInput = Input.GetAxisRaw(VERTICAL);
            xMouseInput = Input.GetAxis(MOUSE_X);
            yMouseInput = Input.GetAxis(MOUSE_Y);

            // Handle jump and crouch inputs for ground movement
            if (!isSwimming)
            {
                if (Input.GetKeyDown(jumpKey)) { Jump(); }
                if (Input.GetKeyDown(crouchKey)) { StartSlide(); }
                if (Input.GetKeyUp(crouchKey)) { EndSlide(); }
            }
            else // Handle swimming inputs
            {
                if (Input.GetKey(jumpKey)) { SwimUp(); }
                if (Input.GetKey(crouchKey)) { SwimDown(); }
            }
        }
    }

    // Handles movement while on a slope (implementation needed)
    private void SlopeCounterMovement()
    {
        // Logic for handling movement adjustments on slopes
    }

    // Manages player movement based on input and ground conditions
    public Vector3 moveDirection;
    private void Movement()
    {
        // Calculate movement direction based on input
        moveDirection = transform.TransformDirection(new Vector3(xInput, 0, yInput)).normalized * speed;

        // Check for slope detection and adjust movement accordingly
        if (SlopeDetection())
        {
            Vector3 temp = Vector3.Cross(slopeHit.normal, Vector3.down);
            var groundSlopeDir = Vector3.Cross(temp, slopeHit.normal);
            var groundSlopeAngle = Vector3.Angle(slopeHit.normal, Vector3.up);
            if (groundSlopeAngle < 45)
            {
                moveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal); // Adjust movement to slope
            }
        }

        // Adjust velocity based on movement conditions
        if (isGrounded && !isSliding)
        {
            if (xInput != 0 || yInput != 0)
            {
                rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(moveDirection.x, moveDirection.y > 0 ? moveDirection.y * slopeCounter : 0, moveDirection.z), 0.05f);
            }
            else
            {
                rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(0, rb.velocity.y, 0), 0.5f); // Slow down when not moving
            }
        }
        else if (isSliding && isGrounded)
        {
            // Additional logic for sliding state (implementation needed)
        }
        else if (!isSliding)
        {
            if (xInput != 0 || yInput != 0)
            {
                rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z), 0.01f);
            }
        }
    }

    // Manages swimming movement by applying forces based on input
    private void SwimMovement()
    {
        rb.AddForce(Vector3.down * 1); // Apply a downward force to simulate buoyancy
        moveDirection = cam.transform.TransformDirection(new Vector3(xInput, 0, yInput)).normalized * speed / 2; // Adjust speed while swimming
        // Adjust velocity based on input while swimming
        if (xInput != 0 || yInput != 0)
        {
            rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z), 0.01f);
        }
    }

    // Makes the player jump if grounded and allowed to jump
    private void Jump()
    {
        if (canJump)
        {
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse); // Apply upward force for jump
            canJump = false; // Reset jump ability
        }
    }

    // Applies upward force for swimming up
    private void SwimUp()
    {
        rb.AddForce(Vector3.up * jumpHeight / 4); // Apply a small upward force
    }

    // Applies downward force for swimming down
    private void SwimDown()
    {
        rb.AddForce(Vector3.up * jumpHeight / -4); // Apply a small downward force
    }

    // Initiates sliding when crouch key is pressed
    private void StartSlide()
    {
        isSliding = true; // Set sliding state
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(transform.localScale.x, 0.4f, transform.localScale.z), 0.5f); // Adjust scale for sliding
        if (isGrounded)
        {
            rb.AddForce(rb.velocity.normalized * rb.velocity.magnitude / 2f, ForceMode.Impulse); // Apply force to start sliding
        }
    }

    // Ends the slide based on player input
    private void EndSlide()
    {
        isSliding = false; // Reset sliding state
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(transform.localScale.x, 0.6f, transform.localScale.z), 0.5f); // Reset scale
    }

    Vector3 yLook;
    
    // Manages the player and player camera rotation based on the input of the mouse
    private void Look()
    {
        float yCamView = -yMouseInput * LookSens; // Calculate camera view change
        transform.Rotate(0, xMouseInput * LookSens, 0); // Rotate player based on mouse input
        yLook = new Vector3(Mathf.Clamp(yLook.x + yCamView, -90, 90), 0, 0); // Clamp vertical look angle
        cam.transform.localRotation = Quaternion.Euler(yLook); // Apply rotation to camera

        // Adjust field of view based on player speed
        var cams = FindObjectsOfType<Camera>();
        foreach (var x in cams)
        {
            x.fieldOfView = Mathf.Lerp(cam.fieldOfView, 90 + (rb.velocity.magnitude / 2), 0.1f);
        }
    }

    float lastSpeed;
    
    // This does the ground check with a sphere check and runs a coyote time for the jump
    private void GroundCheck()
    {
        if (!isSwimming)
        {
            if (Physics.CheckSphere(groundCheck.position, groundCheckRange, notPlayerMask))
            {
                if (isGrounded == false)
                {
                    isGrounded = true; // Set grounded state
                    canJump = true; // Allow jumping
                }
            }
            else
            {
                if (isGrounded == true)
                {
                    isGrounded = false; // Reset grounded state
                    StartCoroutine(CoyoteTime()); // Start coyote time for jump
                }
            }
            lastSpeed = rb.velocity.y; // Store last vertical speed
        }
        else
        {
            isGrounded = false; // Reset grounded state if swimming
        }
    }

    // Coyote timer (this lets the player jump after going off the edge of a platform)
    private IEnumerator CoyoteTime()
    {
        yield return new WaitForSeconds(0.20f); // Wait for coyote time duration
        if (!isGrounded)
        {
            canJump = false; // Disable jump if not grounded
        }
    }
}
