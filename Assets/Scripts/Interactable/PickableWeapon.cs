using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject weapon;

    private GameObject _weaponSpawn;
    // Start is called before the first frame update
    public void Start()
    {
        _weaponSpawn = GameObject.FindGameObjectWithTag("WeaponSlot");
    }
    public void OnTriggerEnter(Collider collision) 
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("dentro if");
            Instantiate(weapon,  _weaponSpawn.transform.position, _weaponSpawn.transform.rotation, _weaponSpawn.transform.parent);
            Destroy(gameObject);
        }
    }
}
