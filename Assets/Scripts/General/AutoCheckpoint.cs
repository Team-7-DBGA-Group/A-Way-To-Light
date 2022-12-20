using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCheckpoint : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Transform checkpoint;

    private void OnTriggerEnter(Collider other)
    {
        SpawnManager.Instance.SetNewSpawnPoint(checkpoint.position);
    }
}
