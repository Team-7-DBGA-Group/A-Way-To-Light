using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Boss : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private List<BossPillar> pillars = new List<BossPillar>();
    [SerializeField]
    private BossBarrier barrier;
    [SerializeField]
    private Animator animator;

    private FSMSystem FSM;

    public void SpawnPillars()
    {
        foreach (BossPillar pillar in pillars)
            pillar.Spawn();
    }

    public void ActivateBarrier()
    {
        foreach(BossPillar pillar in pillars)
            pillar.ResetPillar();

        barrier.ActivateBarrier();
    }

    public void DeactivateBarrier() => barrier.DeactivateBarrier();

    private void Awake()
    {
        SetupFSM();
    }

    private void SetupFSM()
    {
        FSM = new FSMSystem();
        // TO-DO: Add States here ...
    }
}
