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

    public void Interact()
    {
        animator.SetTrigger("Move");
    }

    public void StopMoving()
    {
        animator.SetTrigger("Stop");
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
