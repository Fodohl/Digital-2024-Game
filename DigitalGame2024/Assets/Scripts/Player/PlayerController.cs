using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float lookSpeed = 2f;
    public Camera playerCamera;
    private float y;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        float moveVertical = -Input.GetAxis("Horizontal");
        float moveHorizontal = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        movement = transform.TransformDirection(movement);
        
        rb.MovePosition(transform.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        Vector3 rotation = playerCamera.transform.localEulerAngles;
        y += mouseX * Time.deltaTime;
        rotation.x -= mouseY * Time.deltaTime;
        rotation.x = Mathf.Clamp(rotation.x, -90f, 90f);

        playerCamera.transform.localEulerAngles = rotation;

        transform.rotation = Quaternion.Euler(0, y, 0);
    }
}

