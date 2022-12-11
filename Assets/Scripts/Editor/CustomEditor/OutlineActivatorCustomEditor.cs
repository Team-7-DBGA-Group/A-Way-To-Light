using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(OutlineActivator), true)]
public class OutlineActivatorCustomEditor : Editor
{
    private void OnSceneGUI()
    {
        OutlineActivator activator = (OutlineActivator)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(activator.transform.position, Vector3.up, Vector3.forward, 360, activator.OutlineRadius);
    }
}
