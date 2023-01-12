using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : BasicEnemy
{
    [Header("Boss Enemy References")]
    [SerializeField]
    private GameObject smokeParticle;

    [Header("Boss Enemy Settings")]
    [SerializeField]
    private float targetSpawnHeight = 2.5f;
    [SerializeField]
    private float targetSmokeHeight = 4.0f;
    [SerializeField]
    private float spawnSpeed = 2.0f;

    private bool _isSpawning = false;
    private GameObject _smokeInstance = null;

    public void Spawn()
    {
        if (_isSpawning)
            return;

        _isSpawning = true;
        _smokeInstance = Instantiate(smokeParticle, new Vector3(transform.position.x, targetSmokeHeight, transform.position.z), Quaternion.identity);
    }

    protected override void Start()
    {
        base.Start();
        Spawn();
    }

    protected override void Update()
    {
        base.Update();
        if (_isSpawning)
        {
            transform.position += Vector3.up * spawnSpeed * Time.deltaTime;
            if (transform.position.y >= targetSpawnHeight)
            {
                _isSpawning = false;
                StartCoroutine(WaitForAction(0.6f, () => { Rise(); }));
                Destroy(_smokeInstance, 0.6f);
            }
        }
    }

    private IEnumerator WaitForAction(float sec, Action OnActionComplete)
    {
        yield return new WaitForSeconds(sec);
        OnActionComplete?.Invoke();
    }
}
