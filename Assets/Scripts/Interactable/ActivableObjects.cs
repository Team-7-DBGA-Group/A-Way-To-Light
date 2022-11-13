using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivableObjects : MonoBehaviour, IInteractable
{
    [Header("Objects References")]
    [SerializeField]
    private List<GameObject> objects = new List<GameObject>();

    public void Interact()
    {
        ActiveObjects();
    }

    private void Start()
    {
        foreach (GameObject obj in objects)
            obj.SetActive(false);
    }

    private void ActiveObjects()
    {
        if(objects.Count <= 0)
            return;

        foreach(GameObject obj in objects)
            obj.SetActive(true);
    }
}
