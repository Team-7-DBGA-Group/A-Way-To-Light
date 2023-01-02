using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivableObjects : InteractableObject
{
    [Header("Objects References")]
    [SerializeField]
    private List<GameObject> objects = new List<GameObject>();

    public override void Interact()
    {
        base.Interact();
        ActiveObjects();
    }

    private void ActiveObjects()
    {
        if(objects.Count <= 0)
            return;

        foreach(GameObject obj in objects)
            obj.SetActive(true);
    }
}
