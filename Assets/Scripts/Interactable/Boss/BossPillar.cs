using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossPillar : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject gem;
    [SerializeField]
    private GameObject smokeParticle;

    [Header("Pillar Settings")]
    [SerializeField]
    private float targetPillarHeight = 2.5f;
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

    public void ResetPillar()
    {
        if (gem == null)
            return;
        if (gem.activeSelf)
            return;

        gem.SetActive(true);
    }

    private void Start()
    {
        gem.SetActive(false);
    }

    private void Update()
    {
        if (_isSpawning)
        {
            transform.position += Vector3.up * spawnSpeed * Time.deltaTime;
            if(transform.position.y >= targetPillarHeight)
            {
                _isSpawning = false;
                StartCoroutine(WaitForAction(0.6f, () => { gem.SetActive(true); }));
                Destroy(_smokeInstance,0.6f);
            }
        }
    }

    private IEnumerator WaitForAction(float sec, Action OnActionComplete)
    {
        yield return new WaitForSeconds(sec);
        OnActionComplete?.Invoke();
    }
}
