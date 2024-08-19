using System.Collections;
using UnityEngine;
using Alteruna;
using System.Collections.Generic;

public class PlayerAnimation : AttributesSync
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject gunHolder;
    private List<Animator> gunAnimator = new List<Animator>();
    [SerializeField] private RigidbodySynchronizable rb;
    [SerializeField] private float rateOfChange = 0.25f;
    private void Awake(){
        for (int i = 0; i < gunHolder.transform.childCount; i++)
        {
            gunAnimator.Add(gunHolder.transform.GetChild(i).GetComponent<Animator>());
        }
        
    }
    private void Update()
    {
        if (gunAnimator.Count < 1){
            for (int i = 0; i < gunHolder.transform.childCount; i++)
            {
                gunAnimator.Add(gunHolder.transform.GetChild(i).GetComponent<Animator>());
            }
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
            foreach (var item in gunAnimator)
            {  
                if (item.gameObject.activeSelf){
                    item.SetBool("Moving", true);
                }
            }
        }
        else if (forwardVelocity < -2)
        {
            animator.SetFloat("Y", Mathf.Lerp(animator.GetFloat("Y"), -1, rateOfChange));
            foreach (var item in gunAnimator)
            {  
                if (item.gameObject.activeSelf){
                    item.SetBool("Moving", true);
                }
            }
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
            foreach (var item in gunAnimator)
            {  
                if (item.gameObject.activeSelf){
                    item.SetBool("Moving", false);
                }
            }
        }
    }
}
