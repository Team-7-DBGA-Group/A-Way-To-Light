using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int Damage { get => damage; }
    public int Durability { get => durability; }

    [SerializeField]
    private int damage = 1;
    [SerializeField]
    private int durability = 3;
}
