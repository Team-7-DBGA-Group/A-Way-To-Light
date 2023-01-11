using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Discharge : MonoBehaviour
{
    [SerializeField]
    private GameObject ParticleDischarge;
    public float radius = 10f;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Discharge();
        }
       

    }
    private void Discharge()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var item in colliders)
        {
            if (item.gameObject.GetComponentInChildren<InteractableObject>())
                item.GetComponentInChildren<InteractableObject>().Interact();
            if (item.gameObject.GetComponentInChildren<ActiveAnimation>())
                item.GetComponentInChildren<ActiveAnimation>().Interact();
            if (item.gameObject.GetComponentInChildren<TransportableObject>())
                item.GetComponentInChildren<TransportableObject>().Interact();
            if (item.gameObject.GetComponentInChildren<CartInteractable>())
                item.GetComponentInChildren<CartInteractable>().Interact();
            if (item.gameObject.GetComponentInChildren<DestroyableFragmentObject>())
                item.GetComponentInChildren<DestroyableFragmentObject>().Interact();
            if (item.gameObject.GetComponentInChildren<TurnOnLight>())
                item.GetComponentInChildren<TurnOnLight>().Interact();

        }
        
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log(other.gameObject.name);
    //    InteractableObject interactableObj = null;
    //    if (other.gameObject.TryGetComponent(out interactableObj))
    //    {

    //        //InteractableObject[] interactables = other.gameObject.GetComponents<InteractableObject>();

    //        //foreach (InteractableObject interactable in interactables)
    //            interactableObj.Interact();

    //    }
    //}
}
