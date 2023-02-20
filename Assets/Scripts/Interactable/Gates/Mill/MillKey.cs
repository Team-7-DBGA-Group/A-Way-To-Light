using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MillKey : GateKey
{
    [SerializeField]
    private GameObject ropeMillToDestroy;
    protected override void CustomInteraction()
    {
        if (ropeMillToDestroy != null)
            Destroy(ropeMillToDestroy);
    }
}
