using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable, IDataPersistence
{
    public bool IsInteractionActive { get; set; }

    [Header("Save System")]
    [SerializeField]
    protected string ID;

    [ContextMenu("Generate GUID for ID")]
    private void GenerateGuid()
    {
        ID = System.Guid.NewGuid().ToString();
    }

    public virtual void Interact()
    {
        IsInteractionActive = true;
    }

    public void LoadData(GameData data)
    {
        bool isActive = false;
        data.InteractablesActivated.TryGetValue(ID, out isActive);
        if(isActive)
            Interact();
    }

    public void SaveData(GameData data)
    {
        if(data.InteractablesActivated.ContainsKey(ID))
            data.InteractablesActivated.Remove(ID);

        data.InteractablesActivated.Add(ID, IsInteractionActive);
    }
}
