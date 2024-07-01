using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private RBMovement rb;
    private void Update(){
        var y = rb.y;
        if (y > 0) y = rb.sprinting? 2 : 1;
        animator.SetFloat("X", rb.x);
        animator.SetFloat("Y", rb.y);
    }
}
