using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;

public class Player : Actor
{
    public bool IsWeaponEquip { get => _currentEquipWeapon != null; }

    [Header("Player Settings")]
    [SerializeField]
    private float dropRayDistance = 1.6f;

    private Weapon _currentEquipWeapon = null;

    private Vector3[] _directions = new Vector3[4];

    public void Equip(Weapon w)
    {
        _currentEquipWeapon = w;
    }

    public void Unequip()
    {
        if (!IsWeaponEquip)
            return;

        RaycastHit hit;
        foreach (Vector3 direction in _directions)
        {
            if (Physics.Raycast(transform.position + new Vector3(0, 1f, 0), direction, out hit, dropRayDistance))
            {
                Debug.Log("Hit with " + hit.collider.gameObject.name);
                continue;
            }
            if (Physics.Raycast(transform.position + (-gameObject.transform.forward * dropRayDistance) + new Vector3(0, 1f, 0), Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                GameObject pickableObj = Instantiate(_currentEquipWeapon.PickablePrefab, hit.point, Quaternion.identity);
                pickableObj.transform.forward = hit.collider.gameObject.transform.up;
                pickableObj.transform.position += new Vector3(0, 0.07f, 0);
                pickableObj.GetComponent<PickableWeapon>().PassData(_currentEquipWeapon);
                break;
            }
        }

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

    private void Update()
    {
        UpdateRaysDirections();

        foreach (Vector3 direction in _directions)
        {
            Debug.DrawRay(transform.position + (direction * dropRayDistance) + new Vector3(0, 1f, 0), Vector3.down * 10.0f, Color.red);
            Debug.DrawRay(transform.position + new Vector3(0, 1f, 0), direction * dropRayDistance, Color.cyan);
        }
        
    }

    private void UpdateRaysDirections()
    {
        _directions[0] = -transform.forward;
        _directions[1] = -transform.right;
        _directions[2] = transform.right;
        _directions[3] = transform.forward;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("DeathZone"))
        {
            StartCoroutine(CODie());
        }
    }

    IEnumerator CODie()
    {
        yield return new WaitForSeconds(1.0f);
        Die();
    }
}
