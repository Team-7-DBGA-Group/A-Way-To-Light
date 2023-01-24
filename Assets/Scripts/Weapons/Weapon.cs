using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int Damage { get => damage; }
    public int Durability { get => durability; }
    public GameObject PickablePrefab { get => pickablePrefab; }

    [Header("Weapon Settings")]
    [SerializeField]
    private int damage = 1;
    [SerializeField]
    private int durability = 3;
    [SerializeField]
    private bool canBeDestroyed = true;

    [Header("References")]
    [SerializeField]
    private GameObject pickablePrefab;

    [Header("Sounds")]
    [SerializeField]
    private AudioClip onBreakSound;

    public void PassData(PickableWeapon p)
    {
        damage = p.Damage;
        durability = p.Durability;
    }

    public void RemoveDurability(int amount)
    {
        if (!canBeDestroyed)
            return;

        if (amount <= 0)
            return;

        durability -= amount;

        if (durability <= 0)
        {
            AudioManager.Instance.PlaySound(onBreakSound);
            Destroy(gameObject);
        }
    }
}
