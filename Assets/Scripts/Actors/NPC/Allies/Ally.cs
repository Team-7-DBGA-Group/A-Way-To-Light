using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : NPC
{
    [Header("Ally References")]
    [SerializeField]
    private DialogueTrigger dialogueTrigger;

    public override void Die()
    {
        // It can't die...
        // ... unless!
    }

    public override void Interact()
    {
        if (IsAlive)
            return;
        Rise();
        dialogueTrigger.enabled = true;
    }

    public override void LoadData(GameData data)
    {
        bool isAlive = false;
        data.AlliesAlive.TryGetValue(ID, out isAlive);
        if (isAlive)
            Interact();
    }

    public override void SaveData(GameData data)
    {
        if (data.AlliesAlive.ContainsKey(ID))
            data.AlliesAlive.Remove(ID);

        data.AlliesAlive.Add(ID, IsAlive);
    }

    protected override void Awake()
    {
        base.Awake();
        dialogueTrigger.enabled = false;
    }
}
