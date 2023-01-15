using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoInteractable : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private List<GameObject> interactables;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var interactable in interactables)
                interactable.GetComponent<IInteractable>().Interact();
        }
    }
}
