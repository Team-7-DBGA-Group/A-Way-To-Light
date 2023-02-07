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
    [Header("Sounds references")]
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip onMovingEffect;

    private bool _canMove = false;
    private Vector3 _endPosition;

    private void Awake()
    {
        if (audioSource)
        {
            audioSource.clip = onMovingEffect;
        }
    }

    public void StartFloating()
    {
        audioSource.Play();
        _endPosition = new Vector3(transform.position.x, transform.position.y + height, transform.position.z);
        _canMove = true;
    }

    private void Update()
    {
        if (!_canMove)
            return;

        transform.position = Vector3.MoveTowards(transform.position, _endPosition, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, _endPosition) < 0.001f)
        {
            _canMove = false;
            audioSource.Stop();
        }
    }
}
