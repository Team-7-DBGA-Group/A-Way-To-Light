using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableFragmentObject : InteractableObject
{
    [Header("References")]
    [SerializeField]
    private GameObject fragmentedObject;

    [Header("Settings")]
    [SerializeField]
    private float clearTime = 4.0f;

    public override void Interact()
    {
        base.Interact();
        Explode();
    }

    private void Explode()
    {
        GameObject fragObj = Instantiate(fragmentedObject, this.transform.position + fragmentedObject.transform.position, this.transform.rotation);
        
        Destroy(fragObj, clearTime);
        this.gameObject.SetActive(false);
    }
}
