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

    protected override void Awake()
    {
        base.Awake();
        dialogueTrigger.enabled = false;
    }
}
