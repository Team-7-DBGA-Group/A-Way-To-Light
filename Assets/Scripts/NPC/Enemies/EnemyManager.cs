using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class EnemyManager : Singleton<EnemyManager>
{
    public static event Action OnCombatEnter;
    public static event Action OnCombatExit;
    public int CurrentEnemiesInCombat { get => _inCombatEnemies.Count; }

    private Dictionary<int, Enemy> _inCombatEnemies = new Dictionary<int, Enemy>();

    public void RegisterInCombatEnemy(int hash, Enemy enemy)
    {
        if (_inCombatEnemies.ContainsKey(hash))
            return;

        if (CurrentEnemiesInCombat == 0)
            OnCombatEnter?.Invoke();

        _inCombatEnemies.Add(hash, enemy);
        CustomLog.Log(CustomLog.CustomLogType.SYSTEM, "New enemy register as InCombat with hash" + hash);
    }

    // Call it when an enemy is dead
    public void DeregisterInCombatEnemy(int hash)
    {
        _inCombatEnemies.Remove(hash);
        if (CurrentEnemiesInCombat <= 0)
            OnCombatExit?.Invoke();
        CustomLog.Log(CustomLog.CustomLogType.SYSTEM, "Enemy removed from InCombat with hash" + hash);
    }
}
