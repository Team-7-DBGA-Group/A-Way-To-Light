using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Actor : MonoBehaviour
{
    public event Action<int> OnHealthDamaged;
    public event Action<int> OnHealthHealed;
    public event Action<int> OnHealthInitialized;

    public int MaxHealth { get { return maxHealth; } protected set { maxHealth = value; } }
    public int CurrentHealth { get; protected set; }
    
    [Header("Health settings")]
    [SerializeField]
    private int maxHealth = 3;

    public abstract void Die();

    public void TakeDamage(int damage)
    {
        if(damage<=0)
            return;
        CurrentHealth -= damage;
        OnHealthInitialized?.Invoke(damage);
        if (CurrentHealth <= 0)
            Die();
    }

    public void ResetHealth()
    {
        CurrentHealth = MaxHealth;
        OnHealthHealed(MaxHealth);
    }

    public void Heal(int healAmount)
    {
        if (healAmount <= 0)
            return;
        CurrentHealth += healAmount;
        OnHealthInitialized?.Invoke(healAmount);
        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;
    }

    private void Start()
    {
        CurrentHealth = MaxHealth;
        OnHealthInitialized?.Invoke(MaxHealth);
    }
}
