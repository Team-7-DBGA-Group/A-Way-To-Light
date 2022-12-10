using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public partial class DevControlEditorTool : EditorWindow
{
    [MenuItem("Tools/Dev Control Editor")]
    public static void StartEditor()
    {
        DevControlEditorTool devContolEditor = EditorWindow.GetWindow<DevControlEditorTool>("Dev Editor", true);
        devContolEditor.Show();
    }

    private void OnGUI()
    {
        DrawScreenshotTool();
    }

    private void OnEnable()
    {
        _screenshotPath = EditorPrefs.GetString("ScreenshotPath");
    }

    private void OnDisable()
    {
        EditorPrefs.SetString("ScreenshotPath", _screenshotPath);
    }
}
