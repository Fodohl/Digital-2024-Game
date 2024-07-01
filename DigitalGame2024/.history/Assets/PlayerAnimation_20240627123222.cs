using System.Collections;
using UnityEngine;
using Alteruna;

public class PlayerAnimation : AttributesSync
{
    [SerializeField] private Animator animator;
    private RigidbodySynchronizable rb;
    private void Awake(){   
        rb = GetComponent<RigidbodySynchronizable>();
    }
    private void Update(){
        if (GetComponent<Alteruna.Avatar>().IsMe){
            print(rb.velocity.magnitude);
            animator.SetFloat("Sprinting", rb.velocity.magnitude);
            animator.SetFloat("X", Mathf.Lerp(animator.GetFloat("X"), rb.velocity.x, 0.1f));
            animator.SetFloat("Y", Mathf.Lerp(animator.GetFloat("Y"), rb.velocity.y, 0.1f));
        }
    }
}
