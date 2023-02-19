using UnityEngine;

public class TrapDoorGate : TransportableObject
{
    public bool IsTrapdoorOpen { get => _trapdoorOpen; }

    [Header("References")]
    [SerializeField]
    private GameObject gate;
    [SerializeField]
    private GameObject trapdoor;
    [SerializeField]
    private Animator trapdoorAnimator;

    private bool _trapdoorOpen = false;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Interact()
    {
        if (IsMoving)
            return;

        base.Interact();

        if (_trapdoorOpen)
        {
            trapdoorAnimator.SetTrigger("Close");
            _trapdoorOpen = false;
        }
        else
        {
            trapdoorAnimator.SetTrigger("Open");
            _trapdoorOpen = true;
        }
    }


}
