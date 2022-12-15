#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;

public class DevTool : EditorWindow
{
    private List<RadioButton> _radios = new List<RadioButton>();
    private List<RadioButton> _runtimeRadios = new List<RadioButton>();
    private List<RadioButton> _editorRadios = new List<RadioButton>();

    [MenuItem("Tools/Dev Tool")]
    public static void ShowExample()
    {
        DevTool wnd = GetWindow<DevTool>();
        wnd.titleContent = new GUIContent("DevTool");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Utils/Tools/UIToolkit/DevTool/DevTool.uxml");
        VisualElement treeFromUXML = visualTree.Instantiate();
        root.Add(treeFromUXML);

        SetupButtonsHandlers();
        
        if (Application.isPlaying)
        {
            SetActiveRunTimeRadios(true);
            SetActiveEditorRadios(false);
        }
        else
        {
            SetActiveRunTimeRadios(false);
            SetActiveEditorRadios(true);
        }
        
        EditorApplication.playModeStateChanged += CheckStatesOnExit;
    }

    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= CheckStatesOnExit;
    }

    private void SetupButtonsHandlers()
    {
        Button screenshotToolBtn = rootVisualElement.Q<Button>("screenshotBtn");
        Button vfxActivatorBtn = rootVisualElement.Q<Button>("vfxActivatorBtn");
        Button teleportBtn = rootVisualElement.Q<Button>("teleportBtn");
        Button multiPlacingBtn = rootVisualElement.Q<Button>("multiPlacingBtn");

        screenshotToolBtn.RegisterCallback<ClickEvent>((ClickEvent evt) => { ScreenshotTool.ShowEditor();  });
        vfxActivatorBtn.RegisterCallback<ClickEvent>((ClickEvent evt) => { VFXActivatorTool.ShowEditor(); });
        teleportBtn.RegisterCallback<ClickEvent>((ClickEvent evt) => { TeleportTool.ShowEditor(); });
        multiPlacingBtn.RegisterCallback<ClickEvent>((ClickEvent evt) => { MultipleObjectPlacing.ShowEditor(); });
        
        UQueryBuilder<RadioButton> radios = rootVisualElement.Query<RadioButton>();

        _radios = radios.ToList();

        foreach(RadioButton radio in _radios)
        {
            if (radio.name.Contains("runtime"))
            {
                _runtimeRadios.Add(radio);
            }
            else if (radio.name.Contains("editor"))
            {
                _editorRadios.Add(radio);
            }
        }

        SetRadios(Color.green);
    }

    private void CheckStatesOnExit(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            SetActiveRunTimeRadios(false);
            SetActiveEditorRadios(true);
        }
    }

    private void SetRadios(Color color)
    {
        foreach (RadioButton radio in _radios)
        {
            radio.Q<VisualElement>("unity-checkmark").style.backgroundColor = color;
            radio.SetEnabled(false);
        }
    }

    private void SetActiveRunTimeRadios(bool active)
    {
        foreach (RadioButton radio in _runtimeRadios)
        {
            radio.value = active;
        }
    }

    private void SetActiveEditorRadios(bool active)
    {
        foreach (RadioButton radio in _editorRadios)
        {
            radio.value = active;
        }
    }
}
#endif