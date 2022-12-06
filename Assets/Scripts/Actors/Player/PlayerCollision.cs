using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Transport"))
            other.transform.gameObject.GetComponent<TransportableObject>().SetTransport(this.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Transport"))
            other.transform.gameObject.GetComponent<TransportableObject>().StopTransport();
    }
}
