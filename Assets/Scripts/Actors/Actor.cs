using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    public int MaxHealth { get { return maxHealth; } protected set { maxHealth = value; } }
    public int CurrentHealth { get; protected set; }
    
    [Header("Health settings")]
    [SerializeField]
    private int maxHealth = 0;

    public abstract void Die();

    public void TakeDamage(int damage)
    {
        if(damage<=0)
            return;
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
            Die();
    }

    public void ResetHealth()
    {
        CurrentHealth = MaxHealth;
    }

    public void Heal(int healAmount)
    {
        if (healAmount <= 0)
            return;
        CurrentHealth += healAmount;
        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;
    }

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }
}
