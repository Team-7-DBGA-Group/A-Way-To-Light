using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDarkShot : MonoBehaviour, IInteractable
{
    [Header("Settings")]
    [SerializeField]
    private float followSpeed = 5.0f;

    private GameObject _target = null;

    public void DestroyShot() => Destroy(this.gameObject);

    private void Start()
    {
        // _target = SpawnManager.Instance.PlayerObj;
        StartCoroutine(Wait());
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
    }

    private void FollowTarget()
    {
        if (_target == null)
            return;

        transform.position = Vector3.MoveTowards(this.transform.position, _target.transform.position, Time.deltaTime * followSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = null;
        if(other.gameObject.TryGetComponent(out player))
        {
            player.TakeDamage(1, this.gameObject);
        }

        DestroyShot();
    }

    // Test Only
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(3.0f);
        _target = SpawnManager.Instance.PlayerObj;
    }

    public void Interact()
    {
        DestroyShot();
    }
}
