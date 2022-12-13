using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Utils;
using Unity.EditorCoroutines.Editor;
using System.Collections;

public class ScreenshotTool : EditorWindow
{
    private string _path = "";
    private bool _isCutsceneBorder = false;
    private bool _isTakingCutsceneScreenshot = false;
    private Animator _cutsceneAnimator;

    public static void ShowEditor()
    {
        ScreenshotTool wnd = GetWindow<ScreenshotTool>();
        wnd.titleContent = new GUIContent("ScreenshotTool");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Utils/Tools/UIToolkit/ScreenshotTool/ScreenshotTool.uxml");
        VisualElement treeFromUXML = visualTree.Instantiate();
        root.Add(treeFromUXML);

        // Update Textfield
        TextField pathField = rootVisualElement.Q<TextField>("pathField");
        pathField.SetValueWithoutNotify(_path);

        SetupButtonsHandler();
    }

    private void OnEnable()
    {
        _path = EditorPrefs.GetString("ScreenshotPath");
    }

    private void OnDisable()
    {
        EditorPrefs.SetString("ScreenshotPath", _path);
    }

    private void SetupButtonsHandler()
    {
        Button screenshotBtn = rootVisualElement.Q<Button>("takeScreenshotBtn");
        screenshotBtn.RegisterCallback<ClickEvent>(ScreenshotClick);

        Button pathBtn = rootVisualElement.Q<Button>("pathBtn");
        pathBtn.RegisterCallback<ClickEvent>(PathClick);
    }

    private void ScreenshotClick(ClickEvent evt)
    {
        TextField pathField = rootVisualElement.Q<TextField>("pathField");
        _path = pathField.text;

        Toggle cutsceneBorderToggle = rootVisualElement.Q<Toggle>("cutsceneBorderToggle");
        _isCutsceneBorder = cutsceneBorderToggle.value;

        if(_isCutsceneBorder)
            TakeScreenshotCutscene(_path);
        else
            TakeScreenshot(_path);
    }

    private void PathClick(ClickEvent evt)
    {
        _path = EditorUtility.OpenFolderPanel("Save Screenshots Path", System.IO.Directory.GetCurrentDirectory(), "");
        TextField pathField = rootVisualElement.Q<TextField>("pathField");
        pathField.SetValueWithoutNotify(_path);
    }

    private void TakeScreenshot(string path)
    {
        if ((!string.IsNullOrEmpty(path)) && (!path.EndsWith('/')))
        {
            path += "/";
        }

        string fileName = "";
        if (string.IsNullOrEmpty(path))
        {
            fileName = "Unity_AWayToLight_" +
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

        if (!UnityEngine.Application.isPlaying)
        {
            TakeScreenshot(path);
            return;
        }


        if (CutsceneManager.Instance.IsPlayingCutscene)
        {
            TakeScreenshot(path);
            return;
        }

        _cutsceneAnimator = FindObjectOfType<UICutscenePanel>().GetComponent<Animator>();
        object coroutineObject = new object();
        EditorCoroutineUtility.StartCoroutine(COCutsceneScreenshot(path), coroutineObject);
    }

    private IEnumerator COCutsceneScreenshot(string path)
    {
        _cutsceneAnimator.SetTrigger("Open");
        _isTakingCutsceneScreenshot = true;
        yield return new EditorWaitForSeconds(1.5f);
        TakeScreenshot(path);
        yield return new EditorWaitForSeconds(0.5f);
        _cutsceneAnimator.SetTrigger("Close");
        _isTakingCutsceneScreenshot = false;
    }

}