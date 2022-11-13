using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        DestroyThis();
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }
}