using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class UISceneTransition : MonoBehaviour
{ 
    public event Action OnOpenTransitionEnded;
    public event Action OnCloseTransitionEnded;

    [Header("References")]
    [SerializeField]
    private Animator animator;

    public void InvokeCloseTransitionEnded() => OnCloseTransitionEnded?.Invoke();
    public void InvokeOpenTransitionEnded() => OnOpenTransitionEnded?.Invoke();
    
    // Might need a bool for executing once
    public void Open()
    {
        animator.SetTrigger("Open");
    }

    public void Close()
    {
        animator.SetTrigger("Close");
    }
}
