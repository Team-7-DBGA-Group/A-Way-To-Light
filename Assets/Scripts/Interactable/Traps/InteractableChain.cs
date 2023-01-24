using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableChain : InteractableObject
{
    [Header("References")]
    [SerializeField]
    private GameObject rope;

    private bool _interacted = false;

    public override void Interact()
    {
        if (_interacted)
            return;
        
        GameObject ropeToDestroy = rope;
        Destroy(ropeToDestroy);
        _interacted = true;
    }
}
