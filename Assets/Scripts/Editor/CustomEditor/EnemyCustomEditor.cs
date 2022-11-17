using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Enemy), true)]
public class EnemyCustomEditor : Editor
{
    private void OnSceneGUI()
    {
        Enemy enemy = (Enemy)target;
        Handles.color = Color.red;
        Handles.DrawWireArc(enemy.transform.position, Vector3.up, Vector3.forward, 360, enemy.AttackRange);
        Handles.color = new Color32(134, 52, 235, 255);
        Handles.DrawWireArc(enemy.transform.position, Vector3.up, Vector3.forward, 360, enemy.CombatRange);
    }
}
