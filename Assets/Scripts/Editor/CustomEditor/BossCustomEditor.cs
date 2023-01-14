using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Boss), true)]
public class BossCustomEditor : Editor
{
    private void OnSceneGUI()
    {
        Boss boss = (Boss)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(boss.transform.position, Vector3.up, Vector3.forward, 360, boss.BossRange);
    }
}
