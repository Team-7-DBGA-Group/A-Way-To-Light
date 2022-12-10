using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public partial class DevControlEditorTool : EditorWindow
{
    private SerializedObject _serializedObject;

    [MenuItem("Tools/Dev Control Editor")]
    public static void StartEditor()
    {
        DevControlEditorTool devContolEditor = EditorWindow.GetWindow<DevControlEditorTool>("Dev Editor", true);
        devContolEditor.Show();
    }

    private void OnGUI()
    {
        DrawScreenshotTool();

        _serializedObject.ApplyModifiedProperties();
    }

    private void OnEnable()
    {
        // Serialization
        _serializedObject = new SerializedObject(this);
        _cutsceneAnimatorSerialized = _serializedObject.FindProperty("cutsceneAnimator");

        _screenshotPath = EditorPrefs.GetString("ScreenshotPath");
    }

    private void OnDisable()
    {
        EditorPrefs.SetString("ScreenshotPath", _screenshotPath);
    }
}
