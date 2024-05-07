using Alteruna;
using UnityEngine;

public class Jump : MonoBehaviour
{
    RigidbodySynchronizable rb;
    public float jumpStrength = 2;
    public event System.Action Jumped;

    [SerializeField, Tooltip("Prevents jumping when the transform is in mid-air.")]
    GroundCheck groundCheck;


    void Reset()
    {
        // Try to get groundCheck.
        groundCheck = GetComponentInChildren<GroundCheck>();
    }

    void Awake()
    {
        // Get rb.
        rb = GetComponent<RigidbodySynchronizable>();
    }
    private bool lastJump = false;
    void LateUpdate()
    {

        // Jump when the Jump button is pressed and we are on the ground.
        var pressed = GetComponent<FirstPersonMovement>().input.KeyValues[1];
        if (pressed && (!groundCheck || groundCheck.isGrounded))
        {
            rb.AddForce(Vector3.up * 100 * jumpStrength);
            Jumped?.Invoke();
        }
        lastJump = pressed
    }
}
