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

    [Header("AudioSource reference")]
    [SerializeField]
    private AudioSource audioSource;

    [Header("Sounds")]
    [SerializeField]
    private AudioClip throwAttackSound;
    [SerializeField]
    private AudioClip floatingAttackSound;
    [SerializeField]
    private AudioClip avoidAttackSound;

    private GameObject _target = null;
    private bool _isFollowingTarget = true;
    private Vector3 _lastTargetPos = Vector3.zero;
    private float _zOffset = 0.0f;
    private bool _stop = false;

    public void DestroyShot() => Destroy(this.gameObject);
    public void Interact()
    {
        DestroyShot();
    }

    public void SetForwardOffset(float offset)
    {
        _zOffset = offset;
    }

    private void Start()
    {
        _target = SpawnManager.Instance.PlayerObj;
        AudioManager.Instance.PlaySound(throwAttackSound);
        audioSource.clip = floatingAttackSound;
        audioSource.Play();
        Destroy(this.gameObject, destroyTime);
    }

    private void OnEnable()
    {
        GameManager.OnPause += HandlePause;
        Player.OnPlayerDie += DestroyShot;
        AudioManager.OnChangedSoundVolume += ChangeSoundVolume;
    }

    private void OnDisable()
    {
        Player.OnPlayerDie -= DestroyShot;
        GameManager.OnPause -= HandlePause;
        AudioManager.OnChangedSoundVolume -= ChangeSoundVolume;
    }

    private void Update()
    {
        if (_stop)
            return;

        FollowTarget();
        FollowDirection();
    }

    private void FollowTarget()
    {
        if (_target == null || !_isFollowingTarget)
            return;

        transform.position = Vector3.MoveTowards(this.transform.position, _target.transform.position + new Vector3(0.0f, yOffset, _zOffset), Time.deltaTime * followSpeed);

        SwitchTarget();
    }

    private void SwitchTarget()
    {
        if (Vector3.Distance(_target.transform.position, this.transform.position) > graceDistance)
            return;

        _isFollowingTarget = false;
        _lastTargetPos = _target.transform.position + new Vector3(0.0f, yOffset, _zOffset);
    }

    private void FollowDirection()
    {
        if (_isFollowingTarget)
            return;

        transform.position += Time.deltaTime * followSpeed * (_lastTargetPos - transform.position).normalized;
    }

    private void HandlePause(bool isPause)
    {
        _stop = isPause;
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

    private void ChangeSoundVolume()
    {
        if (audioSource != null)
            audioSource.volume = AudioManager.Instance.GetSoundVolume();
    }

    private void OnDestroy()
    {
        AudioManager.Instance.PlaySound(avoidAttackSound);
    }
}
