using System.Collections.Generic;
using Alteruna;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    [HideInInspector]
    public Alteruna.Avatar avatar;
    public float speed = 5;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public KeyCode Jump;
    public KeyCode Sprint;

    private Rigidbody rb;
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();



    void Awake()
    {
        avatar = GetComponent<Alteruna.Avatar>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!avatar.IsMe)
            return;
        IsRunning = canRun && Input.GetKey(KeyCode.LeftShift);

        float targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        Vector2 targetVelocity =new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        targetVelocity *= targetMovingSpeed;

        rb.velocity = transform.rotation * new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.y);
    }
}