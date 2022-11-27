using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class EnemyManager : Singleton<EnemyManager>
{
    public static event Action OnCombatEnter;
    public static event Action OnCombatExit;
    public static event Action<int> OnEnemyInCombatRegistered;
    public static event Action<int> OnEnemyInCombatDeregistered;

    public int CurrentEnemiesInCombat { get => _inCombatEnemies.Count; }
    public Dictionary<int, Enemy> InCombatEnemies { get=> _inCombatEnemies; }

    private Dictionary<int, Enemy> _inCombatEnemies = new Dictionary<int, Enemy>();
    public void RegisterInCombatEnemy(int hash, Enemy enemy)
    {
        if (_inCombatEnemies.ContainsKey(hash))
            return;

        if (CurrentEnemiesInCombat == 0)
            OnCombatEnter?.Invoke();

        _inCombatEnemies.Add(hash, enemy);
        OnEnemyInCombatRegistered?.Invoke(hash);

        CustomLog.Log(CustomLog.CustomLogType.SYSTEM, "New enemy register as InCombat with hash" + hash);
    }

    public void DeregisterInCombatEnemy(int hash)
    {
        _inCombatEnemies.Remove(hash);
        if (CurrentEnemiesInCombat <= 0)
            OnCombatExit?.Invoke();

        OnEnemyInCombatDeregistered?.Invoke(hash);
        CustomLog.Log(CustomLog.CustomLogType.SYSTEM, "Enemy removed from InCombat with hash" + hash);
    }
}
