using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using CartoonFX;
using System.Collections.Generic;
using Utils;

public class VFXActivatorTool : EditorWindow
{
    public static void ShowEditor()
    {
        VFXActivatorTool wnd = GetWindow<VFXActivatorTool>();
        wnd.titleContent = new GUIContent("VFXActivatorTool");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Utils/Tools/UIToolkit/VFXActivatorTool/VFXActivatorTool.uxml");
        VisualElement treeFromUXML = visualTree.Instantiate();
        root.Add(treeFromUXML);

        SetupButtonHandler();
    }

    private void SetupButtonHandler()
    {
        Button applyBtn = rootVisualElement.Q<Button>("applyBtn");
        applyBtn.RegisterCallback<ClickEvent>(ApplyVFX);
    }

    private void ApplyVFX(ClickEvent evt)
    {
        if (!Application.isPlaying)
            return;

        Toggle toggleActive = rootVisualElement.Q<Toggle>("activeVFXToggle");

        SetVFXActive(toggleActive.value);
    }

    private void SetVFXActive(bool active)
    {
        foreach (CFXR_Effect vfx in Resources.FindObjectsOfTypeAll(typeof(CFXR_Effect)) as CFXR_Effect[])
        {
            if (!EditorUtility.IsPersistent(vfx.transform.root.gameObject) && !(vfx.hideFlags == HideFlags.NotEditable || vfx.hideFlags == HideFlags.HideAndDontSave))
            {
                vfx.gameObject.SetActive(active);
            }
        }
        CustomLog.Log(CustomLog.CustomLogType.SYSTEM, "VFX are now " + (active ? "ON" : "OFF"));
    }
}