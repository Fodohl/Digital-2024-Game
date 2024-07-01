using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna;

public class PlayerAnimation : AttributesSync
{
    [SerializeField] private Animator animator;
    private AnimationSynchronizable animSync;
    private RBMovement rb;
    private float sprintingLast;

    private void Awake(){   
        rb = GetComponent<RBMovement>();
        animSync = animator.GetComponent<AnimationSynchronizable>();
        animSync.Animator = animator;
    }
    private void Update(){
        if (GetComponent<Alteruna.Avatar>().IsMe){
            if(rb.sprinting != sprintingLast){
                BroadcastRemoteMethod("sprintingBool", rb.sprinting);
                sprintingLast = rb.sprinting;
            }
            var y = rb.y;
            animSync.SetFloat("Sprinting", rb.sprinting);
            animSync.SetFloat("X", Mathf.Lerp(animator.GetFloat("X"), rb.x, 0.1f));
            animSync.SetFloat("Y", Mathf.Lerp(animator.GetFloat("Y"), y, 0.1f));
        }
    }
    [SynchronizableMethod]
    private void sprintingBool(float s){
        animator.SetFloat("Sprinting", s);
    }
}
