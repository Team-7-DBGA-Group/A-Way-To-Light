using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MultipleObjectsPlacement : EditorWindow
{
    private GameObject _prefab;
    
    private int _objectsNumber = 0;
    private float _distanceBetweenObjects = 0.0f;

    private Vector3 _direction = Vector3.zero;
    private Vector3 _position = Vector3.zero;
    private Vector3 _rotation = Vector3.zero;

    [MenuItem("Tools/Multiple Objects Placement")]
    public static void StartEditor()
    {
        MultipleObjectsPlacement placementEditorWindow = EditorWindow.GetWindow<MultipleObjectsPlacement>("Placement Editor", true);
        placementEditorWindow.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        
        GUILayout.Label("Multiple Objects Placement Tool", EditorStyles.boldLabel);

        EditorGUILayout.Separator();
        GUILayout.Label("Objects", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Object prefab");
        _prefab = EditorGUILayout.ObjectField(_prefab, typeof(GameObject), true) as GameObject;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Number of Objects");
        _objectsNumber = EditorGUILayout.IntField(_objectsNumber);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Distance between Objects");
        _distanceBetweenObjects = EditorGUILayout.FloatField(_distanceBetweenObjects);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Separator();
        GUILayout.Label("Position, Direction and Rotation", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        _position = EditorGUILayout.Vector3Field("Position: ",_position);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        _direction = EditorGUILayout.Vector3Field("Direction: ", _direction);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        _rotation = EditorGUILayout.Vector3Field("Rotation: ", _rotation);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Separator();
        if (GUILayout.Button("Place"))
        {
            PlaceObjects();
        }

        EditorGUILayout.EndVertical();
    }

    private void PlaceObjects()
    {
        if (_prefab == null)
            return;
        if (_objectsNumber <= 0)
            return;

        Vector3 pos = _position;
        for(int i=0; i<_objectsNumber; ++i)
        {
            Instantiate(_prefab, pos, Quaternion.Euler(_rotation));
            pos = pos + _direction * _distanceBetweenObjects;
        }
            
        Debug.Log("[MultipleObjectsPlacement Tool] Objects placed!");
    }
}
