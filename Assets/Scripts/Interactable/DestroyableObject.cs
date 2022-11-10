using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour, Iinteractable
{
    public void DestroyThis()
    {
        Destroy(gameObject);
    }

    public void Interact()
    {
        DestroyThis();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.K))
        {
            Interact();
        }

    }
}