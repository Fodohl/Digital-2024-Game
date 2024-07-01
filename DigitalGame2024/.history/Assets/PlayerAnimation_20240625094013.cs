using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna;

public class PlayerAnimation : AttributesSync
{
    [SerializeField] private Animator animator;
    private AnimationSynchronizable animSync;
    private RBMovement rb;
    private bool sprintingLast;

    private void Awake(){   
        rb = GetComponent<RBMovement>();
        animSync = animator.GetComponent<AnimationSynchronizable>();
        animSync.Animator = animator;
        rb.y
    }
    private void Update(){
        if (GetComponent<Alteruna.Avatar>().IsMe){
            if(rb.sprinting != sprintingLast){

            }
            var y = rb.y;
            animSync.SetFloat("Sprinting", rb.sprinting);
            animSync.SetFloat("X", Mathf.Lerp(animator.GetFloat("X"), rb.x, 0.1f));
            animSync.SetFloat("Y", Mathf.Lerp(animator.GetFloat("Y"), y, 0.1f));
        }
    }
    [SynchronizableMethod]
    private void sprintingBool(bool s){
        animator.SetBool("Sprinting", s);
    }
}
