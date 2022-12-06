using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TransportableObject : MonoBehaviour, IInteractable
{
    public bool OnTransport { get; private set; }

    [SerializeField]
    private List<Transform> waypoints = new List<Transform>();

    [SerializeField]
    private float objectSpeed = 2f; 

    private GameObject _playerRef;
    private PlayerMovement _playerMovementRef;
    private int _waypointIndex;
    private bool _canObjectMove = false;

    public void Interact()
    {
        if (_canObjectMove)
            return;

        if (_playerRef != null)
        {
            _playerRef.transform.parent = this.gameObject.transform;
            _playerRef.GetComponent<PlayerMovement>().StopMovement();
        }
        _waypointIndex = (_waypointIndex + 1) % waypoints.Count;
        _canObjectMove = true;
    }

    public void setTransport(GameObject player)
    {
        _playerRef = player;
        OnTransport = true;
    }

    public void exitTransport()
    {
        _playerRef = null;
        OnTransport = false;
    }

    private void Start()
    {
        _waypointIndex = 0;
        transform.position = waypoints[_waypointIndex].position;
        transform.LookAt(waypoints[(_waypointIndex + 1) % waypoints.Count].position);
    }

    private void Update()
    {
        if (!_canObjectMove)
            return;

        transform.position = Vector3.MoveTowards(transform.position, waypoints[_waypointIndex].position, objectSpeed * Time.deltaTime);
        
        if(Vector3.Distance(transform.position, waypoints[_waypointIndex].position) < 0.1f)
        {
            transform.LookAt(waypoints[(_waypointIndex + 1) % waypoints.Count].position);
            
            if(_playerRef != null)
            {
                _playerRef.GetComponent<PlayerMovement>().CanMove = true;
                _playerRef.transform.parent = null;
                _playerRef = null;
            }

            _canObjectMove = false;
        }
    }
}
