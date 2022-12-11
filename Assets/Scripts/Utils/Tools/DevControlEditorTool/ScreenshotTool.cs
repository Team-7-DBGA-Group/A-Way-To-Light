using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Utils;
using Unity.EditorCoroutines.Editor;

public partial class DevControlEditorTool : EditorWindow
{
    [SerializeField]
    private Animator cutsceneAnimator = null;

    private SerializedProperty _cutsceneAnimatorSerialized  = null;
    private string _screenshotPath = "";

    private bool _isCutscene = false;
    private bool _isTakingCutsceneScreenshot = false;

    private void DrawScreenshotTool()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Screenshot Tool", EditorStyles.boldLabel);
        EditorGUILayout.Separator();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Save path: ");
        _screenshotPath = EditorGUILayout.TextField(_screenshotPath);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Cutscene borders: ");
        _isCutscene = EditorGUILayout.Toggle(_isCutscene);
        EditorGUILayout.EndHorizontal();

        if (_isCutscene)
        {
            _serializedObject.Update();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_cutsceneAnimatorSerialized, true);
            EditorGUILayout.EndHorizontal();
        }    

        EditorGUILayout.Separator();
        if (GUILayout.Button("Take Screenshot"))
        {
            if (_isCutscene)
                TakeScreenshotCutscene(_screenshotPath);
            else
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
    
    private void TakeScreenshotCutscene(string path)
    {
        if (_isTakingCutsceneScreenshot)
            return;

        if (!Application.isPlaying)
        {
            TakeScreenshot(_screenshotPath);
            return;
        }

        
        if(CutsceneManager.Instance.IsPlayingCutscene)
        {
            TakeScreenshot(_screenshotPath);
            return;
        }

        object coroutineObject = new object();
        EditorCoroutineUtility.StartCoroutine(COCutsceneScreenshot(), coroutineObject);
    }

    private IEnumerator COCutsceneScreenshot()
    {
        cutsceneAnimator.SetTrigger("Open");
        _isTakingCutsceneScreenshot = true;
        yield return new EditorWaitForSeconds(1.5f);
        TakeScreenshot(_screenshotPath);
        yield return new EditorWaitForSeconds(0.5f);
        cutsceneAnimator.SetTrigger("Close");
        _isTakingCutsceneScreenshot = false;
    }
}
