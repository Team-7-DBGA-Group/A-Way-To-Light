#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using CartoonFX;
using System.Collections.Generic;
using Utils;

public class ActivatorTool : EditorWindow
{
    public static void ShowEditor()
    {
        if (!Application.isPlaying)
            return;

        ActivatorTool wnd = GetWindow<ActivatorTool>();
        wnd.titleContent = new GUIContent("ActivatorTool");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Utils/Tools/UIToolkit/ActivatorTool/ActivatorTool.uxml");
        VisualElement treeFromUXML = visualTree.Instantiate();
        root.Add(treeFromUXML);

        SetupButtonHandler();
    }

    private void SetupButtonHandler()
    { 
        Button applyBtn = rootVisualElement.Q<Button>("applyBtn");
        applyBtn.RegisterCallback<ClickEvent>(ApplyChanges);
    }

    private void ApplyChanges(ClickEvent evt)
    {
        if (!Application.isPlaying)
            return;

        Toggle toggleVFXActive = rootVisualElement.Q<Toggle>("activeVFXToggle");
        Toggle toggleAutoDialogueActive = rootVisualElement.Q<Toggle>("activeAutoDialogueToggle");

        SetVFXActive(toggleVFXActive.value);
        SetAutoDialogueActive(toggleAutoDialogueActive.value);
    }

    private void SetVFXActive(bool active)
    { 
        foreach (CFXR_Effect vfx in Resources.FindObjectsOfTypeAll(typeof(CFXR_Effect)) as CFXR_Effect[])
        {
            if (!EditorUtility.IsPersistent(vfx.transform.root.gameObject) && !(vfx.hideFlags == HideFlags.NotEditable || vfx.hideFlags == HideFlags.HideAndDontSave))
            {
                if (vfx.GetComponent<LightVFX>() || vfx.gameObject.name.Contains("Wind")) // Add other elements to be ignored
                    continue;
                vfx.gameObject.SetActive(active);
            }
        }
        CustomLog.Log(CustomLog.CustomLogType.SYSTEM, "VFXs are now " + (active ? "ON" : "OFF"));
    }

    private void SetAutoDialogueActive(bool active)
    {
        foreach (AutoDialogueTrigger trigger in Resources.FindObjectsOfTypeAll(typeof(AutoDialogueTrigger)) as AutoDialogueTrigger[])
        {
            if (!EditorUtility.IsPersistent(trigger.transform.root.gameObject) && !(trigger.hideFlags == HideFlags.NotEditable || trigger.hideFlags == HideFlags.HideAndDontSave))
            {
                if (active)
                {
                    trigger.SetCanEnableDialogue(true);
                    trigger.EnableTriggerDialogue();
                }
                else
                {
                    trigger.SetCanEnableDialogue(false);
                    trigger.DisableTriggerDialogue();
                }
                    
            }
        }
        CustomLog.Log(CustomLog.CustomLogType.SYSTEM, "AutoDialogues are now " + (active ? "ON" : "OFF"));
    }
}
#endif