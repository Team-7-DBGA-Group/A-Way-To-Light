using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    public int MaxHealth { get { return maxHealth; } private set { maxHealth = value; } }
    public int CurrentHealth { get; private set; }
    
    [Header("Health settings")]
    [SerializeField]
    private int maxHealth = 0;


    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        if(CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void ResetHealth()
    {
        CurrentHealth = MaxHealth;
    }

    public void Heal(int healAmount)
    {
        CurrentHealth += healAmount;
        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;
    }

    public abstract void Die();

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }
}
