using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveAnimation : MonoBehaviour, Iinteractable
{
    private Animator animator;
    [Header("Inserire nome dello stato dell'animator da attivare")]
    [Tooltip("Attenzione alle maiuscole")][SerializeField]private string NomeStato = "Nome stato";
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void PlayThis()
    {
        animator.Play(NomeStato);
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

