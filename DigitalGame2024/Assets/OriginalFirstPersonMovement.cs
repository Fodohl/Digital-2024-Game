using System.Collections;
using TMPro;
using UnityEngine;
using Alteruna;
using System;

public class OriginalFirstPersonMovement : CommunicationBridge
{
    //Rgidbody that controls the player
    private Rigidbody rb;
    //The main camera of the player
    [SerializeField] private Camera cam;
    //visable player mesh
    [SerializeField] private GameObject[] playerMesh;
    //The position at which the ground check is performed
    [SerializeField] private Transform groundCheck;
    //the hight of the ground check
    [SerializeField] private float groundCheckRange;
    //A layermask the doesn't include the player
    [SerializeField] private LayerMask notPlayerMask;
    //look sensitivity
    [SerializeField] private float LookSens = 3f;
    //player speed
    [SerializeField] private float speed = 8000f;
    //player jump height
    [SerializeField] private float jumpHeight = 10f;
    //key codes set in inspector
    [SerializeField] private KeyCode jumpKey, crouchKey;
    //the grounded state of the player
    public bool isGrounded;
    //for logic behind jumping
    private bool canJump;
    //bool to determin whether the player is sliding
    public bool isSliding;
    //input variables
    private float xInput, yInput, xMouseInput, yMouseInput;
    //a bool that tells us if the player dies so that we can run different logic
    private bool dead = false;
    //alteruna avatar
    private Alteruna.Avatar avatar;
    private Vector3 slopeMoveDirection;
    [SerializeField] private float slopeCounter;
    private bool setUp = false;
    private bool isSwimming = false;
    private float waterHeight = -8.3f;


    //basic player set up (cursor and setting rigidbody)
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        avatar = GetComponent<Alteruna.Avatar>();
    }

    private void Update()
    {
        if (!setUp){
            if (!avatar.IsMe){
                GetComponentInChildren<AudioListener>().enabled = false;
            } else {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                foreach (var item in playerMesh)
                {
                    item.SetActive(false);
                }
                setUp = true;
            }
        }
        if (transform.position.y < waterHeight){
            EndSlide();
            isSwimming = true;
            rb.useGravity = false;
            rb.drag = 0.5f;
        }else{
            isSwimming = false;
            rb.useGravity = true;
            rb.drag = 0f;
        }
        if (avatar.IsMe && !isSwimming){
            Movement();
            Look();
            PlayerInput();
        } else if (avatar.IsMe && isSwimming){
            SwimMovement();
            Look();
            PlayerInput();
        }
        GroundCheck();
    }
    RaycastHit slopeHit;
    private bool SlopeDetection(){
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, 2)){
            if (slopeHit.normal != Vector3.up){
                return true;
            }else{
                return false;
            }
        }
        return false;
    }
    //manages the player input and sets the variale/runs a method
    private void PlayerInput(){
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        xMouseInput = Input.GetAxis("Mouse X");
        yMouseInput = Input.GetAxis("Mouse Y");
        if (!isSwimming){
            if (Input.GetKeyDown(jumpKey)){Jump();}
            if (Input.GetKeyDown(crouchKey)){StartSlide();}
            if (Input.GetKeyUp(crouchKey)){EndSlide();}
        } else {
            if (Input.GetKey(jumpKey)){SwimUp();}
            if (Input.GetKey(crouchKey)){SwimDown();}
        }

    }

    //handles movement on slope
    private void SlopeCounterMovement(){
    }
    //manages the movement of the player based on the input
    public Vector3 moveDirection;
    private void Movement(){
        moveDirection = transform.TransformDirection(new Vector3(xInput, 0 , yInput)).normalized * speed;
        if (SlopeDetection()){
            moveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
        }
        if (isGrounded && !isSliding){
            if (xInput != 0 || yInput != 0){
                rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(moveDirection.x, moveDirection.y > 0 ? moveDirection.y * slopeCounter : 0, moveDirection.z), 0.05f);
            }else{
                rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(0, rb.velocity.y, 0), 0.5f);
            }
        } else if (isSliding && isGrounded){
        } else if (!isSliding) {
            if (xInput != 0 || yInput != 0){
                rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z), 0.01f);
            }
        }
    }
    private void SwimMovement(){
        rb.AddForce(Vector3.down * 1);
        moveDirection = cam.transform.TransformDirection(new Vector3(xInput, 0 , yInput)).normalized * speed / 2;
        if (xInput != 0 || yInput != 0){
            rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z), 0.01f);
        }
    }
    //makes the player jump if the player is grounded
    private void Jump(){
        if (canJump){
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            canJump = false;
        }
    }
    private void SwimUp(){
        rb.AddForce(Vector3.up * jumpHeight / 4);
    }
    private void SwimDown(){
        rb.AddForce(Vector3.up * jumpHeight / -4);
    }
    //starts the slide based on player input
    private void StartSlide(){
        isSliding = true;
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(transform.localScale.x, 0.4f, transform.localScale.z), 0.5f);
        if (isGrounded){
            rb.AddForce(rb.velocity.normalized * rb.velocity.magnitude / 2f, ForceMode.Impulse);
        }
    }
    //ends the slide based on player input
    private void EndSlide(){
        isSliding = false;
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(transform.localScale.x, 0.6f, transform.localScale.z), 0.5f);
    }
    Vector3 yLook;
    //manages the player and player camera rotation based on the input of the mouse
    private void Look(){
        float yCamView = -yMouseInput * LookSens;
        transform.Rotate(0, xMouseInput * LookSens, 0);
        yLook = new Vector3(Mathf.Clamp(yLook.x+yCamView, -90, 90), 0, 0);
        cam.transform.localRotation = Quaternion.Euler(yLook);
        var cams = FindObjectsOfType<Camera>();
        foreach (var x in cams)
        {
            x.fieldOfView = Mathf.Lerp(cam.fieldOfView, 90+(rb.velocity.magnitude / 2), 0.1f);  
        }
        
    }
    float lastSpeed;
    //this does the groundcheck with a sphere check and runs a coyote time for the jump
    private void GroundCheck(){
        if (Physics.CheckSphere(groundCheck.position, groundCheckRange, notPlayerMask)){
            if (isGrounded == false){
                isGrounded = true;
                canJump = true;
            }
        } else {
            if (isGrounded == true){
                isGrounded = false;
                StartCoroutine(CoyoteTime());
            }
        }
        lastSpeed = rb.velocity.y;
    }
    //coyote timer (this lets the player jump after going of the edge of a platform)
    private IEnumerator CoyoteTime(){
        yield return new WaitForSeconds(0.20f);
        if (!isGrounded){
            canJump = false;
        }
    }
}
