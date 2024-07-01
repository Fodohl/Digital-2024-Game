using System.Collections;
using UnityEngine;
using Alteruna;

public class PlayerAnimation : AttributesSync
{
    [SerializeField] private Animator animator;
    [SerializeField ]private RigidbodySynchronizable rb;
    private void Update(){
        
        if (rb.velocity.x > 2){
            animator.SetFloat("X", Mathf.Lerp(animator.GetFloat("X"), 1, 0.1f));
        }else if(rb.velocity.x < -2){
            animator.SetFloat("X", Mathf.Lerp(animator.GetFloat("X"), -1, 0.1f));
        }
        if (rb.velocity.y > 2){
            if (rb.velocity.y > 8){
                animator.SetFloat("Y", Mathf.Lerp(animator.GetFloat("Y"), 2, 0.1f));
                animator.SetFloat("Sprinting", Mathf.Lerp(animator.GetFloat("Sprinting"), 2, 0.1f));
            }
            animator.SetFloat("Y", Mathf.Lerp(animator.GetFloat("Y"), 1, 0.1f));
        }else if(rb.velocity.y < -2){
            animator.SetFloat("Y", Mathf.Lerp(animator.GetFloat("Y"), -1, 0.1f));
        }
    }
}
