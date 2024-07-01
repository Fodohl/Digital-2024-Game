using System.Collections;
using UnityEngine;
using Alteruna;

public class PlayerAnimation : AttributesSync
{
    [SerializeField] private Animator animator;
    [SerializeField ]private RigidbodySynchronizable rb;
    private void Update()
{
    Vector3 forwardDirection = rb.transform.forward;
    float forwardVelocity = Vector3.Dot(rb.velocity, forwardDirection);

    if (forwardVelocity > 2)
    {
        animator.SetFloat("Y", Mathf.Lerp(animator.GetFloat("Y"), 1, 0.1f));
        if (forwardVelocity > 8)
        {
            animator.SetFloat("Y", Mathf.Lerp(animator.GetFloat("Y"), 2, 0.1f));
            animator.SetFloat("Sprinting", Mathf.Lerp(animator.GetFloat("Sprinting"), 2, 0.1f));
        }
    }
    else if (forwardVelocity < -2)
    {
        animator.SetFloat("Y", Mathf.Lerp(animator.GetFloat("Y"), -1, 0.1f));
    }
    else
    {
        animator.SetFloat("Y", Mathf.Lerp(animator.GetFloat("Y"), 0, 0.1f));
    }

    // Similarly, you can get the velocity in the right direction (perpendicular to forward)
    Vector3 rightDirection = rb.transform.right;
    float rightVelocity = Vector3.Dot(rb.velocity, rightDirection);

    if (rightVelocity > 2)
    {
        animator.SetFloat("X", Mathf.Lerp(animator.GetFloat("X"), 1, 0.1f));
    }
    else if (rightVelocity < -2)
    {
        animator.SetFloat("X", Mathf.Lerp(animator.GetFloat("X"), -1, 0.1f));
    }
    else
    {
        animator.SetFloat("X", Mathf.Lerp(animator.GetFloat("X"), 0, 0.1f));
    }
}
}
