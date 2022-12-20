using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickablesManager : Singleton<PickablesManager>
{
    private Dictionary<Vector3,GameObject> _pickables = new Dictionary<Vector3,GameObject>();
    public void ResetPickables()
    {
        foreach (Vector3 position in _pickables.Keys)
        {
            GameObject obj = Instantiate(_pickables[position], position, Quaternion.identity);
            obj.transform.localRotation = _pickables[position].transform.localRotation;
        }
        _pickables.Clear();
    }

    private void OnEnable()
    {
        PickableWeapon.OnWeaponPick += AddPickable;
    }

    private void OnDisable()
    {
        PickableWeapon.OnWeaponPick -= AddPickable;
    }

    private void AddPickable(Vector3 position, GameObject prefab)
    {
        _pickables.Add(position, prefab);
    }
}
