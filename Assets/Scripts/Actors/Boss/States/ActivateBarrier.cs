using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateBarrier : FSMState
{
    private Animator _animator;
    private Boss _boss;

    public ActivateBarrier(Boss boss, Animator animator)
    {
        _boss = boss;
        _animator = animator;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _animator.SetTrigger("Barrier");
        // Pillars and Barrier are activated at the end of the animation
    }
}
