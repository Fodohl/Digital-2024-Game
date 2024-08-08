using System.Collections;
using UnityEngine;
using Alteruna;

public class PlayerAnimation : AttributesSync
{
    [SerializeField] private Animator animator;
    [SerializeField] private RigidbodySynchronizable rb;
    [SerializeField] private float rateOfChange = 0.25f;
    private void Update()
    {
        Vector3 forwardDirection = rb.transform.forward;
        float forwardVelocity = Vector3.Dot(rb.velocity, forwardDirection);

        if (forwardVelocity > 2)
        {
            animator.SetFloat("Y", Mathf.Lerp(animator.GetFloat("Y"), 1, rateOfChange));
            if (forwardVelocity > 8)
            {
                animator.SetFloat("Y", Mathf.Lerp(animator.GetFloat("Y"), 2, rateOfChange));
                animator.SetFloat("Sprinting", Mathf.Lerp(animator.GetFloat("Sprinting"), 2, rateOfChange));
            }
        }
        else if (forwardVelocity < -2)
        {
            animator.SetFloat("Y", Mathf.Lerp(animator.GetFloat("Y"), -1, rateOfChange));
        }
        else
        {
            animator.SetFloat("Y", Mathf.Lerp(animator.GetFloat("Y"), 0, rateOfChange));
            animator.SetFloat("Sprinting", Mathf.Lerp(animator.GetFloat("Sprinting"), 0, rateOfChange));
        }

        Vector3 rightDirection = rb.transform.right;
        float rightVelocity = Vector3.Dot(rb.velocity, rightDirection);

        if (rightVelocity > 2)
        {
            animator.SetFloat("X", Mathf.Lerp(animator.GetFloat("X"), 1, rateOfChange));
        }
        else if (rightVelocity < -2)
        {
            animator.SetFloat("X", Mathf.Lerp(animator.GetFloat("X"), -1, rateOfChange));
        }
        else
        {
            animator.SetFloat("X", Mathf.Lerp(animator.GetFloat("X"), 0, rateOfChange));
        }
    }
}
