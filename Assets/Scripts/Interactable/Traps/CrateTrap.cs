using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateTrap : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject brokenCrate;

    private void Awake()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject crateToDestroy = Instantiate(brokenCrate, transform.position, transform.rotation);
        crateToDestroy.transform.localScale = transform.localScale;

        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            GetComponent<BoxCollider>().isTrigger = false;
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            GetComponent<BoxCollider>().isTrigger = false;
    }

}
