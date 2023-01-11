using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Boss : MonoBehaviour
{
    public enum Phase
    {
        FirstPhase,
        SecondPhase,
        ThirdPhase,
    }

    [Header("References")]
    [SerializeField]
    private List<BossPillar> pillars = new List<BossPillar>();
    [SerializeField]
    private BossBarrier barrier;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private GameObject darkShotPrefab;
    [SerializeField]
    private Transform darkShotSpawnPos;

    [Header("Boss Settings")]
    [SerializeField]
    private float fireRate = 2.0f;

    private FSMSystem FSM;
    private ActivateBarrier _activateBarrierState;

    private bool _canShoot = true;
    private Phase _currentPhase = Phase.FirstPhase;

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

    public void Shoot()
    {
        if (!_canShoot)
            return;

        _canShoot = false;
        Instantiate(darkShotPrefab, darkShotSpawnPos.position, Quaternion.identity);
        StartCoroutine(COWaitForFireRate());
    }

    public void GoToNextPhase()
    {
        if (_currentPhase == Phase.ThirdPhase)
            return;

        _currentPhase += 1;
    }

    private void Awake()
    {
        SetupFSM();
    }

    private void Update()
    {
        FSM.Update();
    }

    private void SetupFSM()
    {
        FSM = new FSMSystem();
        _activateBarrierState = new ActivateBarrier(this, animator);
        
        FSM.AddState(_activateBarrierState);

        FSM.GoToState(_activateBarrierState);
    }

    private IEnumerator COWaitForFireRate()
    {
        yield return new WaitForSeconds(fireRate);
        _canShoot = true;
    }
}
