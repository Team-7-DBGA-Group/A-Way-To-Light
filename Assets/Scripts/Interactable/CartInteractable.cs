using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartInteractable : InteractableObject
{
    [Header("References")]
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private TransportableObject transportableObject;

    private bool isPlayingAnim = false;
    public override void Interact()
    {
        if(isPlayingAnim)
            return;

        base.Interact();
        animator.SetTrigger("Move");
        isPlayingAnim = true;
    }

    public void StopMoving()
    {
        animator.SetTrigger("Stop");
        isPlayingAnim = false;
    }

    private void OnEnable()
    {
        transportableObject.OnStopTransport += StopMoving;
        //animator.SetTrigger("Stop");
    }

    private void OnDisable()
    {
        transportableObject.OnStopTransport -= StopMoving;
    }
}
