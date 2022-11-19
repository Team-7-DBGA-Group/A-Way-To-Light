using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : NPC
{
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
    }
}
