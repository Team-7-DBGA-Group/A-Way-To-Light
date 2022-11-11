using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableObject : MonoBehaviour, Iinteractable
{
    private ParticleSystem Particle;
    private void Start()
    {
        
        Particle = GetComponent<ParticleSystem>();
        Particle.Stop();
    }
    public void PlayThis()
    {
        Particle.Play();
    }

    public void Interact()
    {
        PlayThis();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.K))
        {
            Interact();
        }

    }
}
