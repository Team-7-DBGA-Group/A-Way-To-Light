using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveAnimation : MonoBehaviour, Iinteractable
{
    Animator animator;
    private void Start()
    {

        animator = GetComponent<Animator>();
    }
    public void PlayThis()
    {
        animator.Play("open");
    }

    public void Interact()
    {
        PlayThis();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.K))
        {
            Interact();
        }

    }
}

