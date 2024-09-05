using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float lookSpeed = 2f;
    public Camera playerCamera;

    private float yaw;
    private float pitch;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        yaw = transform.eulerAngles.y;
        pitch = playerCamera.transform.localEulerAngles.x;
    }

    private void Update()
    {
        RotateCamera();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        movement = transform.TransformDirection(movement);
        
        rb.MovePosition(transform.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
    }
}
