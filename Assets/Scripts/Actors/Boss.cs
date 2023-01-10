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
    [SerializeField]
    private GameObject darkShotPrefab;
    [SerializeField]
    private Transform darkShotSpawnPos;

    [Header("Boss Settings")]
    [SerializeField]
    private float fireRate = 2.0f;

    private FSMSystem FSM;

    private bool _canShoot = true;

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

    private void Awake()
    {
        SetupFSM();
    }

    private void Update()
    {
        // TO-DO: Delete
        Shoot();
    }

    private void SetupFSM()
    {
        FSM = new FSMSystem();
        // TO-DO: Add States here ...
    }

    private IEnumerator COWaitForFireRate()
    {
        yield return new WaitForSeconds(fireRate);
        _canShoot = true;
    }
}
