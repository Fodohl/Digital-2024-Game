using System.Collections.Generic;
using Alteruna;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;
    [HideInInspector]
    public InputSynchronizable input;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public SyncedKey Jump;
    public SyncedKey Sprint;

    private RigidbodySynchronizable rb;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();



    void Awake()
    {
        // Get the rb on this.
        rb = GetComponent<RigidbodySynchronizable>();

        input = GetComponent<InputSynchronizable>();

        Jump.Register(input);
        Sprint.Register(input);
    }

    void FixedUpdate()
    {
        // Update IsRunning from input.
        IsRunning = canRun && input.KeyValues[0];

        // Get targetMovingSpeed.
        float targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        // Get targetVelocity from input.
        Vector2 targetVelocity =new Vector2(input.AxesValues[0], input.AxesValues[1]).normalized;
        targetVelocity *= targetMovingSpeed;

        // Apply movement.
        rb.velocity = transform.rotation * new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.y);
    }
}