using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GateElevation : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private float height = 2f;

    private bool _canMove = false;
    private Vector3 _endPosition;

    public void StartFloating()
    {
        _endPosition = new Vector3(transform.position.x, transform.position.y + height, transform.position.z);
        _canMove = true;
    }

    private void Update()
    {
        if (!_canMove)
            return;

        transform.position = Vector3.MoveTowards(transform.position, _endPosition, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, _endPosition) < 0.001f)
            _canMove = false;
    }
}
