using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : InteractableObject 
{ 
    public override void Interact()
    {
        base.Interact();
        DestroyThis();
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }
}