using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossLantern : MonoBehaviour, IInteractable
{
    public Action OnLanternInteraction;

    private bool _canInteract = true;
    private float _lanternInteractionCD = 10.0f;
    
    public void Interact()
    {
        if(!_canInteract)
            return;

        _canInteract = false;
        OnLanternInteraction?.Invoke();
        StartCoroutine(COWaitLanternInteractCooldown());
    }

    private IEnumerator COWaitLanternInteractCooldown()
    {
        yield return new WaitForSeconds(_lanternInteractionCD);
        _canInteract = true;
    }
}
