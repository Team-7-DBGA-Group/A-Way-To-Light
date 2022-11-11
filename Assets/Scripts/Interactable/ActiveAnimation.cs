using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveAnimation : MonoBehaviour, Iinteractable
{
    private Animator animator;
    [SerializeField]private string NomeAnim = "Nome animazione";
    private void Start()
    {
        
        animator = GetComponent<Animator>();
    }
    public void PlayThis()
    {
        animator.Play(NomeAnim);
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

