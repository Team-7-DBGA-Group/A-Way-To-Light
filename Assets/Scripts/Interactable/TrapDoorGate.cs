using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDoorGate : TransportableObject
{
    [Header("References")]
    [SerializeField]
    private GameObject gate;
    [SerializeField]
    private GameObject trapdoor;

    private bool _trapdoorOpen = false;
    private Animation _trapdoorAnimation;

    private void Awake()
    {
        _trapdoorAnimation = trapdoor.GetComponent<Animation>();
    }

    public override void Interact()
    {
        base.Interact();
        if(!CanMove)
            return;

        if (_trapdoorOpen)
        {
            _trapdoorAnimation["TrapDoorAnimation"].speed = -1;
            _trapdoorAnimation["TrapDoorAnimation"].time = _trapdoorAnimation["TrapDoorAnimation"].length;
            _trapdoorAnimation.Play("TrapDoorAnimation");
            _trapdoorOpen = false;
        }
        else
        {
            _trapdoorAnimation["TrapDoorAnimation"].speed = 1;
            _trapdoorAnimation["TrapDoorAnimation"].time = _trapdoorAnimation["TrapDoorAnimation"].length;
            _trapdoorAnimation.Play("TrapDoorAnimation");
            _trapdoorOpen = true;
        }
    }
}
