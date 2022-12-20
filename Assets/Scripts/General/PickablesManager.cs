using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickablesManager : Singleton<PickablesManager>
{
    [Header("References")]
    [SerializeField]
    private List<PickableWeapon> weapons = new List<PickableWeapon>();

    private Dictionary<Vector3,GameObject> _pickables = new Dictionary<Vector3,GameObject>();

    public void InitPickables()
    {
        foreach(PickableWeapon weapon in weapons)
        {
            AddPickable(weapon.transform.position, weapon.WieldablePrefab.GetComponent<Weapon>().PickablePrefab);
        }
    }

    public void ResetPickables()
    {
        foreach (Vector3 position in _pickables.Keys)
        {
            GameObject obj = Instantiate(_pickables[position], position, Quaternion.identity);
            obj.transform.localRotation = _pickables[position].transform.localRotation;
        }
    }

    private void Start()
    {
        InitPickables();
    }

    private void AddPickable(Vector3 position, GameObject prefab)
    {
        _pickables.Add(position, prefab);
    }
}
