using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Actor : MonoBehaviour
{
    public event Action<int> OnHealthDamaged;
    public event Action<int> OnHealthHealed;
    public event Action<int> OnHealthInitialized;
    public event Action OnKnockbackEnter;
    public event Action OnKnockbackExit;

    public int MaxHealth { get { return maxHealth; } protected set { maxHealth = value; } }
    public int CurrentHealth { get; protected set; }
    
    [Header("Health settings")]
    [SerializeField]
    private int maxHealth = 3;

    [Header("On hit knockback settings")]
    [SerializeField]
    private float knockbackDuration = 0.5f;
    [SerializeField]
    private float knockbackSpeed = 5f;

    public abstract void Die();

    private bool _onKnockback = false;
    private GameObject _lastAttacker = null;

    public void TakeDamage(int damage, GameObject attacker)
    {
        //if(damage<=0)
        //    return;
        CurrentHealth -= damage;
        OnHealthDamaged?.Invoke(damage);
        _lastAttacker = attacker;
        Knockback();
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
        OnHealthHealed?.Invoke(healAmount);
        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;
    }

    protected virtual void Start()
    {
        CurrentHealth = MaxHealth;
        OnHealthInitialized?.Invoke(MaxHealth);
    }

    private void Knockback()
    {
        OnKnockbackEnter?.Invoke();
        StartCoroutine(COKnockback());
    }
    
    private IEnumerator COKnockback()
    {
        _onKnockback = true;
        yield return new WaitForSeconds(knockbackDuration);
        _onKnockback = false;
        OnKnockbackExit?.Invoke();
    }

    private void FixedUpdate()
    {
        if (!_onKnockback)
            return;

        // Attacker -forward as direction
        transform.Translate(transform.worldToLocalMatrix.MultiplyVector(_lastAttacker.transform.forward) * knockbackSpeed * Time.deltaTime);
    }
}
