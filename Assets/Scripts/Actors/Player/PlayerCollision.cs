using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField]
    private AudioClip waterCollisionSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
            AudioManager.Instance.PlaySound(waterCollisionSound);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Transport"))
        {
            if(other.transform.gameObject.GetComponent<TransportableObject>())
                other.transform.gameObject.GetComponent<TransportableObject>().SetTransport(this.gameObject);
            if(other.transform.gameObject.GetComponent<AdvancedTransportableObject>())
                other.transform.gameObject.GetComponent<AdvancedTransportableObject>().SetTransport(this.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Transport"))
            other.transform.gameObject.GetComponent<TransportableObject>().StopTransport();
    }
}
