using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Utils;

public partial class DevControlEditorTool : EditorWindow
{
    private string _screenshotPath = "";

    private void DrawScreenshotTool()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Screenshot Tool", EditorStyles.boldLabel);
        EditorGUILayout.Separator();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Save path: ");
        _screenshotPath = EditorGUILayout.TextField(_screenshotPath);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Separator();
        if (GUILayout.Button("Take Screenshot"))
        {
            TakeScreenshot(_screenshotPath);
        }
        EditorGUILayout.EndVertical();
    }

    private void TakeScreenshot(string path)
    {
        if ((!string.IsNullOrEmpty(path)) && (!path.EndsWith('/')))        
        {
            path += "/";
        }

        string fileName = "";
        if(string.IsNullOrEmpty(path))
        {
           fileName ="Unity_AWayToLight_" +
               System.DateTime.Now.ToString("dd") + "-" +
               System.DateTime.Now.ToString("MM") + "-" +
               System.DateTime.Now.ToString("yyyy") + "-" +
               System.DateTime.Now.ToString("HH") + "-" +
               System.DateTime.Now.ToString("mm") + "-" +
               System.DateTime.Now.ToString("ss") + ".png";

            ScreenCapture.CaptureScreenshot(fileName);

            CustomLog.Log(CustomLog.CustomLogType.SYSTEM, "Screenshot taken and saved at the current path: " + System.IO.Directory.GetCurrentDirectory() + "\\" + fileName);
            
        }
        else
        {
            fileName = path + "Unity_AWayToLight_" +
              System.DateTime.Now.ToString("dd") + "-" +
              System.DateTime.Now.ToString("MM") + "-" +
              System.DateTime.Now.ToString("yyyy") + "-" +
              System.DateTime.Now.ToString("HH") + "-" +
              System.DateTime.Now.ToString("mm") + "-" +
              System.DateTime.Now.ToString("ss") + ".png";

            ScreenCapture.CaptureScreenshot(fileName);

            CustomLog.Log(CustomLog.CustomLogType.SYSTEM, "Screenshot taken and saved at the current path: " + fileName);
        }
    }
}
