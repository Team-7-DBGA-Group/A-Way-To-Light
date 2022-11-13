using UnityEngine;

public class ActiveAnimation : MonoBehaviour, IInteractable
{

    [Header("Animations Settings")]
    [Tooltip("Stato Animazione con Maiuscole!")]
    [SerializeField]
    private string stateName = "";
    [SerializeField]
    private Animator animator;

    public void Interact()
    {
        PlayThis();
    }

    private void PlayThis()
    {
        animator.Play(stateName);
    }
}

