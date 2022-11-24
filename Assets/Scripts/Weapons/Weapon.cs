using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int Damage { get => damage; }
    public int Durability { get => durability; }
    public GameObject PickablePrefab { get => pickablePrefab; }
    [SerializeField]
    private int damage = 1;
    [SerializeField]
    private int durability = 3;

    [SerializeField]
    private GameObject pickablePrefab;


    public void PassData(PickableWeapon p)
    {
        damage = p.Damage;
        durability = p.Durability;
    }
}
