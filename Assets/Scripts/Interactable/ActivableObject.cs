using UnityEngine;

public class ActivableObject : MonoBehaviour, IInteractable
{
    private ParticleSystem _particle;

    private void Start()
    {
        _particle = GetComponent<ParticleSystem>();
        _particle.Stop();
    }
    public void PlayThis()
    {
        _particle.Play();
    }

    public void Interact()
    {
        PlayThis();
    }
}
