using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : FSMState
{
    private Enemy _enemy;

    public EnemyAttackingState(Enemy enemy)
    {
        _enemy = enemy;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        _enemy.Attack();
    }
}
