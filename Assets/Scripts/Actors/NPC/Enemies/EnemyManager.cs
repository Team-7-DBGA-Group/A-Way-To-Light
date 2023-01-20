using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class EnemyManager : Singleton<EnemyManager>
{
    public static event Action OnCombatEnter;
    public static event Action OnCombatExit;
    public static event Action OnBossCombatEnter;
    public static event Action OnBossCombatExit;
    public static event Action<int> OnEnemyInCombatRegistered;
    public static event Action<int> OnEnemyInCombatDeregistered;

    public int CurrentEnemiesInCombat { get => _inCombatEnemies.Count; }
    public Dictionary<int, Enemy> InCombatEnemies { get=> _inCombatEnemies; }

    private Dictionary<int, Enemy> _inCombatEnemies = new Dictionary<int, Enemy>();
    private Dictionary<int, Vector3> _initPositions = new Dictionary<int, Vector3>();

    public void BossEntered() => OnBossCombatEnter?.Invoke();
    public void BossExited() => OnBossCombatExit?.Invoke();

    public void RegisterInCombatEnemy(int hash, Enemy enemy)
    {
        if (_inCombatEnemies.ContainsKey(hash))
            return;

        if (CurrentEnemiesInCombat == 0)
            OnCombatEnter?.Invoke();

        _inCombatEnemies.Add(hash, enemy);
        _initPositions.Add(hash, enemy.gameObject.transform.position);
        OnEnemyInCombatRegistered?.Invoke(hash);

        CustomLog.Log(CustomLog.CustomLogType.SYSTEM, "New enemy register as InCombat with hash" + hash);
    }

    public void DeregisterInCombatEnemy(int hash)
    {
        Enemy enemy = null;
        if (!_inCombatEnemies.TryGetValue(hash, out enemy))
            return;

        // Adjust position for reset
        enemy.transform.position = _initPositions[hash];

        _inCombatEnemies.Remove(hash);
        _initPositions.Remove(hash);

        if (CurrentEnemiesInCombat <= 0)
            OnCombatExit?.Invoke();

        OnEnemyInCombatDeregistered?.Invoke(hash);
        CustomLog.Log(CustomLog.CustomLogType.SYSTEM, "Enemy removed from InCombat with hash" + hash);
    }
}
