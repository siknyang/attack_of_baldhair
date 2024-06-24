using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelAnimationController : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        if (animator != null)
        {
            animator.Play("PanelOpen");
        }
    }
}
