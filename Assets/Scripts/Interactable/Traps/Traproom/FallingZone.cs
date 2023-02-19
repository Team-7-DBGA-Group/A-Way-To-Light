using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FallingZone : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private TrapDoorGate doorGate;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponentInParent<BasicEnemy>() && doorGate.IsTrapdoorOpen)
        {
            other.gameObject.GetComponentInParent<BasicEnemy>().Die();
        }
    }
}
