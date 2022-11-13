using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveAnimation : MonoBehaviour, IInteractable
{
    
    [Header("Animations Settings")]
    [Tooltip("Stato Animazione con Maiuscole!")]
    [SerializeField]
    private string nomeStato = "Nome stato";

    private Animator _animator;

    public void Interact()
    {
        PlayThis();
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void PlayThis()
    {
        _animator.Play(nomeStato);
    }
}

