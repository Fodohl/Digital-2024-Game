using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private RBMovement rb;
    private void Update(){
        animator.SetFloat("X", rb.x);
        animator.SetFloat("Y", rb.y);
    }
}
