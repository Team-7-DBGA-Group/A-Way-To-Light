using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickablesManager : Singleton<PickablesManager>
{
    [Header("References")]
    [SerializeField]
    private List<PickableWeapon> weapons = new List<PickableWeapon>();

    private Dictionary<Vector3,GameObject> _pickables = new Dictionary<Vector3,GameObject>();
    private Dictionary<Vector3, GameObject> _pickablesToSpawn = new Dictionary<Vector3, GameObject>();

    public void InitPickables()
    {
        foreach(PickableWeapon weapon in weapons)
        {
            AddPickable(weapon.transform.position, weapon.WieldablePrefab.GetComponent<Weapon>().PickablePrefab);
        }
    }

    public void ResetPickables()
    {
        foreach (Vector3 position in _pickablesToSpawn.Keys)
        {
            GameObject obj = Instantiate(_pickablesToSpawn[position], position, Quaternion.identity);
            obj.transform.localRotation = _pickablesToSpawn[position].transform.localRotation;
        }
        _pickablesToSpawn.Clear();
    }

    private void Start()
    {
        InitPickables();
    }

    private void OnEnable()
    {
        PickableWeapon.OnWeaponPick += CheckToSpawn;
    }

    private void OnDisable()
    {
        PickableWeapon.OnWeaponPick -= CheckToSpawn;
    }

    private void AddPickable(Vector3 position, GameObject prefab)
    {
        _pickables.Add(position, prefab);
    }

    private void CheckToSpawn(Vector3 position)
    {
        foreach (Vector3 pos in _pickables.Keys)
        {
            if (pos.Equals(position))
            {
                _pickablesToSpawn.Add(pos, _pickables[pos]);
                break;
            }
        }
    }
}
