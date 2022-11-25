using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;

public class Player : Actor
{
    public bool IsWeaponEquip { get => _currentEquipWeapon != null; }

    private Weapon _currentEquipWeapon = null;

    public void Equip(Weapon w)
    {
        _currentEquipWeapon = w;
    }

    public void Unequip()
    {
        if (!IsWeaponEquip)
            return;
        GameObject pickableObj = Instantiate(_currentEquipWeapon.PickablePrefab, gameObject.transform.position + (-gameObject.transform.forward * 1.2f) + new Vector3(0, 0.07f, 0), Quaternion.identity);
        pickableObj.transform.rotation = _currentEquipWeapon.PickablePrefab.transform.rotation;
        pickableObj.GetComponent<PickableWeapon>().PassData(_currentEquipWeapon);

        Destroy(_currentEquipWeapon.gameObject);
        _currentEquipWeapon = null;
    }

    public void DealDamage()
    {
        if (_currentEquipWeapon == null)
            return;
        WeaponDamageDealer wDamageDealer = _currentEquipWeapon.GetComponentInChildren<WeaponDamageDealer>();
        if (wDamageDealer)
        {
            wDamageDealer.StartDealDamage();
        }
    }

    public void StopDealingDamage()
    {
        if (_currentEquipWeapon == null)
            return;
        WeaponDamageDealer wDamageDealer = _currentEquipWeapon.GetComponentInChildren<WeaponDamageDealer>();
        if (wDamageDealer)
        {
            wDamageDealer.EndDealDamage();
        }
    }

    public override void Die()
    {
        CustomLog.Log(CustomLog.CustomLogType.GAMEPLAY, "Game Over!");
        Destroy(gameObject);
    }
}
