using System.Collections;
using UnityEngine;
using Alteruna;

public class PlayerAnimation : AttributesSync
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject gunHolder;
    private Animator gunAnimator;
    [SerializeField] private RigidbodySynchronizable rb;
    [SerializeField] private float rateOfChange = 0.25f;
    private void Awake(){
        gunAnimator = gunHolder.transform.GetChild(0).GetComponent<Animator>();
    }
    private void Update()
    {
        if (!gunAnimator){
            gunAnimator = gunHolder.transform.GetChild(0).GetComponent<Animator>();
        }
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
            gunAnimator.SetBool("Moving", true);
        }
        else if (forwardVelocity < -2)
        {
            animator.SetFloat("Y", Mathf.Lerp(animator.GetFloat("Y"), -1, rateOfChange));
            gunAnimator.SetBool("Moving", true);
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
        if (Mathf.Abs(rb.velocity.magnitude) < 2){
            gunAnimator.SetBool("Moving", false);
        }
    }
}
