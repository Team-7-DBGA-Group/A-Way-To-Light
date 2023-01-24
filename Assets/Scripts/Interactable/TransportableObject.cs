using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System;

public class TransportableObject : InteractableObject
{
    public bool IsTransporting { get; private set; }
    public event Action OnStopTransport;

    [SerializeField]
    private List<Transform> waypoints = new List<Transform>();

    [SerializeField]
    private float objectSpeed = 2f;
    [SerializeField]
    private bool lookAtWaypoints = true;
    
    [Header("Transportable sounds")]
    [SerializeField]
    private AudioClip onMovingSound;

    private GameObject _playerRef;
    private int _waypointIndex;
    private bool _canObjectMove = false;
    private bool _soundFlag = true;

    private AudioSource _audioSource;

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
        if (_canObjectMove)
            return;

        base.Interact();

        if (_playerRef != null)
        {
            _playerRef.transform.parent = this.gameObject.transform;
            _playerRef.GetComponent<PlayerMovement>().StopMovement();
        }
        _waypointIndex = (_waypointIndex + 1) % waypoints.Count;
        _canObjectMove = true;
    }

    public void SetTransport(GameObject player)
    {
        _playerRef = player;
        IsTransporting = true;
    }

    public void StopTransport()
    {
        _playerRef = null;
        IsTransporting = false;
        
    }

    private void Awake()
    {
        if (GetComponent<AudioSource>())
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.clip = onMovingSound;
        }
    }

    private void Start()
    {
        _waypointIndex = 0;
        transform.position = waypoints[_waypointIndex].position;
        if (lookAtWaypoints)
            transform.LookAt(waypoints[(_waypointIndex + 1) % waypoints.Count].position);
    }

    private void Update()
    {
        if (!_canObjectMove)
            return;

        transform.position = Vector3.MoveTowards(transform.position, waypoints[_waypointIndex].position, objectSpeed * Time.deltaTime);
        
        if (_soundFlag && onMovingSound != null && _audioSource != null)
        {
            _audioSource.Play();
            _soundFlag = false;
        }

        if(Vector3.Distance(transform.position, waypoints[_waypointIndex].position) < 0.1f)
        {
            OnStopTransport?.Invoke();
            if (_playerRef != null)
            {
                _playerRef.GetComponent<PlayerMovement>().CanMove = true;
                _playerRef.transform.parent = null;
                _playerRef = null;
            }
            if(lookAtWaypoints)
                transform.LookAt(waypoints[(_waypointIndex + 1) % waypoints.Count].position);
            _canObjectMove = false;

            if(_audioSource != null)
                _audioSource.Stop();

            _soundFlag = true;
        }
    }
    private void ChangeSoundVolume()
    {
        if (_audioSource != null)
            _audioSource.volume = AudioManager.Instance.GetSoundVolume();
    }
}
