using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineActivator : MonoBehaviour
{
    public float OutlineRadius { get => outlineRadius; }

    [SerializeField]
    private float outlineRadius = 5.0f;

    private OutlineWeapon _outlineWeapon = null;

    private void Awake()
    {
        _outlineWeapon = GetComponent<OutlineWeapon>();
        _outlineWeapon.enabled = false;
    }

    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, OutlineRadius, LayerMask.GetMask("Player"));
        if (colliders.Length > 0)
            _outlineWeapon.enabled = true;
        else
            _outlineWeapon.enabled = false;
    }
}
