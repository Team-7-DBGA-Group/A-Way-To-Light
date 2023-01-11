using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDarkShot : MonoBehaviour, IInteractable
{
    [Header("Settings")]
    [SerializeField]
    private float followSpeed = 5.0f;
    [SerializeField]
    private float yOffset = 0.5f;
    [SerializeField]
    private float graceDistance = 1.0f;
    [SerializeField]
    private float destroyTime = 10.0f;

    private GameObject _target = null;
    private bool _isFollowingTarget = true;
    private Vector3 _lastTargetPos = Vector3.zero;

    public void DestroyShot() => Destroy(this.gameObject);
    public void Interact()
    {
        DestroyShot();
    }

    private void Start()
    {
        _target = SpawnManager.Instance.PlayerObj;
        Destroy(this.gameObject, destroyTime);
    }

    private void OnEnable()
    {
        Player.OnPlayerDie += DestroyShot;
    }

    private void OnDisable()
    {
        Player.OnPlayerDie -= DestroyShot;
    }

    private void Update()
    {
        FollowTarget();
        FollowDirection();
    }

    private void FollowTarget()
    {
        if (_target == null || !_isFollowingTarget)
            return;

        transform.position = Vector3.MoveTowards(this.transform.position, _target.transform.position + new Vector3(0.0f, yOffset, 0.0f), Time.deltaTime * followSpeed);

        SwitchTarget();
    }

    private void SwitchTarget()
    {
        if (Vector3.Distance(_target.transform.position, this.transform.position) > graceDistance)
            return;

        _isFollowingTarget = false;
        _lastTargetPos = _target.transform.position + new Vector3(0.0f, yOffset, 0.0f);
    }

    private void FollowDirection()
    {
        if (_isFollowingTarget)
            return;

        transform.position += Time.deltaTime * followSpeed * (_lastTargetPos - transform.position).normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            return;

        Player player = null;
        if(other.gameObject.TryGetComponent(out player))
        {
            player.TakeDamage(1, this.gameObject);
        }

        DestroyShot();
    }
}
