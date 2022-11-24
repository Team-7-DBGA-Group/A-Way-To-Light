using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableWeapon : MonoBehaviour
{
    public int Damage { get => damage; }
    public int Durability { get => durability; }

    [SerializeField]
    private int damage = 1;
    [SerializeField]
    private int durability = 3;

    [SerializeField]
    private GameObject wieldableWeaponPrefab = null;

    public void PassData(Weapon w)
    {
        damage = w.Damage;
        durability = w.Durability;
    }

    public void OnTriggerEnter(Collider collision) 
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            WeaponSlot _weaponSlot = collision.gameObject.GetComponentInChildren<WeaponSlot>();
            
            if (_weaponSlot == null)
                return;

            GameObject weaponObj = Instantiate(wieldableWeaponPrefab,  _weaponSlot.transform);
            weaponObj.transform.localRotation = wieldableWeaponPrefab.transform.localRotation;

            Weapon weapon = weaponObj.GetComponent<Weapon>();
            weapon.PassData(this);

            Player p = collision.gameObject.GetComponent<Player>();
            if (p.IsWeaponEquip)
            {
                p.Unequip();
            }
            p.Equip(weapon);

            Destroy(gameObject);
        }
    }
}
