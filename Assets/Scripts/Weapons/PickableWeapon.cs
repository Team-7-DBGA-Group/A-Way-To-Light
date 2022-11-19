using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableWeapon : MonoBehaviour
{
    // Maybe better to move this logic to player side,
    // because we need an "equip" logic.
    // OR GetComponent<Player>().Equip(Weapon);
    // Maybe having a PlayerEquip script is good, idk.

    [SerializeField]
    private GameObject wieldableWeaponPrefab = null;

    public void OnTriggerEnter(Collider collision) 
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            WeaponSlot _weaponSlot = collision.gameObject.GetComponentInChildren<WeaponSlot>();
            
            if (_weaponSlot == null)
                return;

            GameObject weaponObj = Instantiate(wieldableWeaponPrefab,  _weaponSlot.transform);
            weaponObj.transform.localRotation = wieldableWeaponPrefab.transform.localRotation;

            Destroy(gameObject);
        }
    }
}
