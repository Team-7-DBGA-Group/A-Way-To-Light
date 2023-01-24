using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : InteractableObject
{
    [Header("References")]
    [SerializeField]
    private Transform shotPoint;
    [SerializeField]
    private GameObject lightShotPrefab;
    [SerializeField]
    private GameObject rotatingPoint;
    [SerializeField]
    private AudioSource audioSource;

    [Header("Settings")]
    [SerializeField]
    private float reflactionDelay = 0.1f;

    [Header("Sounds")]
    [SerializeField]
    private AudioClip mirrorRotation;
    [SerializeField]
    private AudioClip mirrorBouncingLight;

    private float _currentRotation = 0;
    private bool _canInteract = false;

    private void OnEnable()
    {
        AudioManager.OnChangedSoundVolume += ChangeSoundVolume;
    }

    private void OnDisable()
    {
        AudioManager.OnChangedSoundVolume -= ChangeSoundVolume;
    }

    public override void Interact()
    {
        StartCoroutine(COReflectShot());
    }

    private void Awake()
    {
        audioSource.clip = mirrorBouncingLight;
        audioSource.volume = AudioManager.Instance.GetSoundVolume();
    }

    private void Start()
    {
        rotatingPoint.transform.Rotate(new Vector3(0, _currentRotation, 0));
    }

    private void Update()
    {
        if (!_canInteract)
            return;

        if (InputManager.Instance.GetInteractPressed())
            NextRotation();
    }

    private void NextRotation()
    {
        AudioManager.Instance.PlaySound(mirrorRotation);
        _currentRotation += 90;
        if (_currentRotation > 360)
            _currentRotation = 0;
        rotatingPoint.transform.eulerAngles = new Vector3(0, _currentRotation, 0);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
            _canInteract = true;
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
            _canInteract = false; 
    }

    IEnumerator COReflectShot()
    {
        yield return new WaitForSeconds(reflactionDelay);
        audioSource.Play();
        GameObject shot = Instantiate(lightShotPrefab, shotPoint.position, Quaternion.identity);
        shot.GetComponent<LightShot>().StartMovingToDirection(shotPoint.forward);
    }

    private void ChangeSoundVolume()
    {
        if (audioSource != null)
            audioSource.volume = AudioManager.Instance.GetSoundVolume();
    }
}
