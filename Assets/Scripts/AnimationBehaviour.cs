using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBehaviour : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ToggleTalkingAnimation()
    {
        if (animator.GetBool("isTalking") == false)
            animator.SetBool("isTalking", true);
        else {
            animator.SetBool("isTalking", false);
        }
    }

    public void ToggleNoAnimation()
    {

    }   
}
