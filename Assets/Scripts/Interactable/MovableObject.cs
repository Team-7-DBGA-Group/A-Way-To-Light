using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : InteractableObject
{
    [Header("Object Movement Settings")]
    [SerializeField]
    private float objectMoveSpeed;
    [SerializeField]
    private List<GameObject> waypoints;

    private int _listIndex = 0;
    private bool _canMoveMiddle = false;

    private bool _doOnce = false;

    public override void Interact()
    {
        if (_doOnce)
            return;

        base.Interact();
        if (waypoints.Count > 0)
            _canMoveMiddle = true;
    }

    public bool MoveObjectEnd(Vector3 endPoint)
    {
        float step = objectMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, endPoint, step);
        float distanceFromEnd = Vector3.Distance(transform.position, endPoint);
        if (distanceFromEnd <= 0.01f)
            return true;
        return false;
    }

    void Update()
    {
        if (_canMoveMiddle)
        {
            if (MoveObjectEnd(waypoints[_listIndex].transform.position))
            {
                _listIndex++;
            }
            if(_listIndex >= waypoints.Count)
            {
                _canMoveMiddle = false;
                _doOnce = true;
            }
        }
    }
}
