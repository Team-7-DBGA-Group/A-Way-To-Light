using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : FSMState
{
    private Animator _animator;
    private Boss _boss;

    public StunState(Boss boss, Animator animator)
    {
        _boss = boss;
        _animator = animator;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _animator.SetTrigger("Stun");
    }

    public override void OnExit()
    {
        base.OnExit();
        _animator.SetTrigger("RecoverStun");
    }
}
