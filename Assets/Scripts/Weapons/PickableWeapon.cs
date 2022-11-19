using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject wieldableWeaponPrefab = null;

    public void OnTriggerEnter(Collider collision) 
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            WeaponSlot _weaponSlot = collision.gameObject.GetComponentInChildren<WeaponSlot>();
            
            if (_weaponSlot == null)
                return;

            Instantiate(wieldableWeaponPrefab,  _weaponSlot.transform.position, _weaponSlot.transform.rotation, _weaponSlot.transform.parent);
            Destroy(gameObject);
        }
    }
}
