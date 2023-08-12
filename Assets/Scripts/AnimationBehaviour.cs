using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBehaviour : MonoBehaviour
{
    Animator animator;  

    public string animationString;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TalkingAnimation()
    {
        animator.SetBool("isTalking", true);
    }  

    public void IdleAnimation()
    {
        animator.SetBool("isTalking", false);        
    }
}
