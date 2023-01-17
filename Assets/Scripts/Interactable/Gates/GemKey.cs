using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemKey : GateKey
{
    [Header("References")]
    [SerializeField]
    private GameObject pointLight;
    [SerializeField]
    private Animator waterAnimator;
    [SerializeField]
    private string animationTrigger;

    private bool _alreadyPlayed = false;

    private void Awake()
    {
        pointLight.SetActive(false);    
    }

    protected override void CustomInteraction()
    {
        pointLight.SetActive(true);

        if (!_alreadyPlayed && animationTrigger != "" && waterAnimator != null)
            waterAnimator.SetTrigger(animationTrigger);

        _alreadyPlayed = true;
    }
}
