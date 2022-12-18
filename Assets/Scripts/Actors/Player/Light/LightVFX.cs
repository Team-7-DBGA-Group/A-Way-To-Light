using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightVFX : MonoBehaviour
{
    private ParticleSystem _particle;
    private Light _light;

    private void Awake()
    {
        _particle = GetComponent<ParticleSystem>();
        _light = GetComponentInChildren<Light>();
    }

    private void Start()
    {
        DisableVFX();
    }

    private void OnEnable()
    {
        PlayerAim.OnAimActive += EnableVFX;
        PlayerAim.OnAimInactive += DisableVFX;
    }

    private void OnDisable()
    {
        PlayerAim.OnAimActive -= EnableVFX;
        PlayerAim.OnAimInactive -= DisableVFX;
    }

    private void EnableVFX()
    {
        _particle.Play();
        _light.enabled = true;
    }

    private void DisableVFX()
    {
        _particle.Stop();
        _light.enabled = false;
    }
}
