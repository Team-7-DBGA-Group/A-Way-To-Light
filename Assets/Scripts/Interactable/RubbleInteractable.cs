using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbleInteractable : GateKey
{
    public bool IsOpen { get; private set; }

    [Header("References")]
    [SerializeField]
    private Animator waterAnimator;
    [SerializeField]
    private string animationTrigger;
    [SerializeField]
    private RubbleInteractable previousRubble;

    [Header("Sounds references")]
    [SerializeField]
    private AudioSource audioSource;

    [Header("Sounds")]
    [SerializeField]
    private AudioClip onBreakSound;

    private bool _alreadyPlayed = false;
    
    private void Awake()
    {
        audioSource.clip = onBreakSound;    
    }

    private void Start()
    {
        IsOpen = false;    
    }

    protected override void CustomInteraction()
    {
        if (_alreadyPlayed)
            return;

        if(previousRubble != null && !previousRubble.IsOpen)
            return;

        IsOpen = true;
        _alreadyPlayed = true;
        waterAnimator.SetTrigger(animationTrigger);

        Destroy(gameObject); 
    }
}
