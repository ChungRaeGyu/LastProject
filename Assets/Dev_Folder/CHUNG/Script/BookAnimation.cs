using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookAnimation : MonoBehaviour
{
    public Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        animator.SetBool("Open", true);
    }
}
