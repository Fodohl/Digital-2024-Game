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
        if (y > 0.9) y = rb.sprinting? 2 : 1;
        animator.SetFloat("X", rb.x);
        animator.SetFloat("Y", y);
    }
}
