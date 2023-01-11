using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingState : FSMState
{
    private Animator _animator;
    private Boss _boss;
    private bool _isMultipleShot = false;

    public ShootingState(Boss boss, Animator animator, bool isMultipleShot)
    {
        _boss = boss;
        _animator = animator;
        _isMultipleShot = isMultipleShot;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!_boss.CanShoot)
        {
            _animator.ResetTrigger("SingleShoot");
            _animator.ResetTrigger("DoubleShoot");
            return;
        }
        // Shots are triggered based on animation
        if (!_isMultipleShot)
        {
            _animator.SetTrigger("SingleShoot");
        }
        else
        {
            _animator.SetTrigger("DoubleShoot");
        }
    }
}
