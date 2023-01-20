using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class UIPausePanel : MonoBehaviour
{
    
    [SerializeField]
    private Animator animator;
    public bool IsOpen { get; private set; }
    public void Open()
    {
        if (IsOpen)
            return;

        IsOpen = true;
        animator.SetTrigger("Open");
    }

    public void Close()
    {
        if (!IsOpen)
            return;

        IsOpen = false;
        animator.SetTrigger("Close");
    }

    private void Start()
    {
        IsOpen = false;
    }
}
