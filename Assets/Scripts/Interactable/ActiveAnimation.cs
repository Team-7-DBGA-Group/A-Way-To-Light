using UnityEngine;

public class ActiveAnimation : InteractableObject
{
    [Header("Animations Settings")]
    [Tooltip("Stato Animazione con Maiuscole!")]
    [SerializeField]
    private string stateName = "";
    [SerializeField]
    private Animator animator;

    public override void Interact()
    {
        base.Interact();
        PlayThis();
    }

    private void PlayThis()
    {
        animator.Play(stateName);
    }
}

