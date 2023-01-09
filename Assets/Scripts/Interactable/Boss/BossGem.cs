using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGem : GateKey
{
    protected override void CustomInteraction()
    {
        this.gameObject.SetActive(false);
    }
}
