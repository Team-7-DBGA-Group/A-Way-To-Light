using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartInteractable : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private TransportableObject transportableObject;

    private bool isPlayingAnim = false;
    public void Interact()
    {
        if(isPlayingAnim)
            return;

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
