using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float destroyTime;

    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
