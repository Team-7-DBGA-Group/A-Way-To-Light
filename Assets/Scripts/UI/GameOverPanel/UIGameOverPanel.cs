using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIGameOverPanel : MonoBehaviour
{
    public static event Action OnPanelClose;

    public bool IsOpen { get; private set; }

    [Header("References")]
    [SerializeField]
    private Animator animator;

    public void InvokePanelCloseEvent() => OnPanelClose?.Invoke();

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
