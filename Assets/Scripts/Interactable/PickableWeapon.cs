using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PickableWeapon : MonoBehaviour
{
    private GameObject WeaponSpawn;
    [SerializeField]
    private GameObject _weapon;

    // Start is called before the first frame update
    public void Start()
    {
        WeaponSpawn = GameObject.FindGameObjectWithTag("WeaponSlot");
        

    }
    public void OnTriggerEnter(Collider collision)
        
    {

        
        if (collision.gameObject.CompareTag("Player"))
        {
            
            Debug.Log("dentro if");
            Instantiate(_weapon,  WeaponSpawn.transform.position, WeaponSpawn.transform.rotation, WeaponSpawn.transform.parent);
            Destroy(gameObject);
        }
    }
}
