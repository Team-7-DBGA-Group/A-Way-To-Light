using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class LightShot : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactableObj = null;
        if(other.gameObject.TryGetComponent<IInteractable>(out interactableObj))
        {
            CustomLog.Log(CustomLog.CustomLogType.GAMEPLAY, "Light Shot interacted with " + other.gameObject.name);
            interactableObj.Interact();

            Destroy(this.gameObject);
        }
    }
}
