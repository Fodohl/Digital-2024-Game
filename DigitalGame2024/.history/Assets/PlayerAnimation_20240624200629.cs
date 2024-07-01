using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private RBMovement rb;
    private void Awake(){   
        rb = GetComponent<RBMovement>();
    }
    private void Update(){
        var y = rb.y;
        animator.SetFloat("Sprinting", rb.sprinting);
        animator.SetFloat("X", Mathf.Lerp(animator.GetFloat("X"), rb.x, 0.1f));
        animator.SetFloat("Y", Mathf.Lerp(animator.GetFloat("Y"), y, 0.1f));
    }
}