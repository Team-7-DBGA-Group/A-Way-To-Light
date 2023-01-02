using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class NPC : Actor, IInteractable, IDataPersistence
{
    public bool IsAlive { get; protected set; }

    [Header("NPC References")]
    [SerializeField]
    protected Animator Animator = null;
    [SerializeField]
    protected MeshRenderer EyesRenderer = null;
    [SerializeField]
    protected Material GlowMat = null;
    [SerializeField]
    protected Material BlackMat = null;

    [Header("Save System")]
    [SerializeField]
    protected string ID;
    [ContextMenu("Generate GUID for ID")]
    private void GenerateGuid() 
    { 
        ID = System.Guid.NewGuid().ToString(); 
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene()); 
    }

    public abstract void LoadData(GameData data);
    public abstract void SaveData(GameData data);

    public abstract void Interact();

    public virtual void Rise()
    {
        if (IsAlive)
            return;
        
        IsAlive = true;
        Animator.SetTrigger("Rise");
        EyesRenderer.material = GlowMat;
    }

    protected virtual void Awake()
    {
        EyesRenderer.material = BlackMat;
        IsAlive = false;
    }
}
