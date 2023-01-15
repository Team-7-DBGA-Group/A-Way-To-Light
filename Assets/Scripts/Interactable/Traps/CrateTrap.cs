using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateTrap : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            GetComponent<BoxCollider>().isTrigger = false;
    }

}
