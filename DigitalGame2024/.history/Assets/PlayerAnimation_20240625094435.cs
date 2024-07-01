using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna;

public class PlayerAnimation : AttributesSync
{
    [SerializeField] private Animator animator;
    private AnimationSynchronizable animSync;
    private RBMovement rb;
    private float sprintingLast, xLast, yLast;

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
            if(rb.x != xLast){
                BroadcastRemoteMethod("xBool", rb.x);
                xLast = rb.x;
            }
            var y = rb.y;
            if(rb.y != yLast){
                BroadcastRemoteMethod("yBool", rb.y);
                yLast = rb.y;
            }
        }
    }
    [SynchronizableMethod]
    private void sprintingBool(float s){
        animator.SetFloat("Sprinting", s);
    }
    [SynchronizableMethod]
    private void xBool(float x){
        animator.SetFloat("X", Mathf.Lerp(animator.GetFloat("X"), rb.x, 0.1f));
    }
    [SynchronizableMethod]
    private void yBool(float y){
        animator.SetFloat("Y", Mathf.Lerp(animator.GetFloat("Y"), y, 0.1f));
    }
}
