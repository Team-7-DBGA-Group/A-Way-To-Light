using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;

public class Player : Actor
{
    public bool IsWeaponEquip { get => currentEquipWeapon != null; }
    
    Weapon currentEquipWeapon = null; 

    public void Equip(Weapon w) 
    {
        currentEquipWeapon = w;
    }

    public void Unequip()
    {
        if (!IsWeaponEquip)
            return;
        GameObject pickableObj = Instantiate(currentEquipWeapon.PickablePrefab, gameObject.transform.position +(-gameObject.transform.forward * 1.2f) + new Vector3(0, 0.07f, 0), Quaternion.identity);
        pickableObj.transform.rotation = currentEquipWeapon.PickablePrefab.transform.rotation;
        pickableObj.GetComponent<PickableWeapon>().PassData(currentEquipWeapon);

        Destroy(currentEquipWeapon.gameObject);
        currentEquipWeapon = null;

    }

    public void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.K))
        {
            Unequip();
        }
    }

    public override void Die()
    {
        CustomLog.Log(CustomLog.CustomLogType.GAMEPLAY, "Game Over!");
        Destroy(gameObject);
    }
}
