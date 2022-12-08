using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnManager : Singleton<SpawnManager>
{
    public static event Action<GameObject> OnPlayerSpawn;

    [Header("References")]
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private Transform startSpawnPoint;

    private Vector3 _currentSpawnPoint = Vector3.zero;
    private GameObject _playerObj = null;

    public void SetNewSpawnPoint(Vector3 position)
    {
        _currentSpawnPoint = position;
    }

    private void Start()
    {
        _currentSpawnPoint = startSpawnPoint.transform.position;
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        _playerObj = Instantiate(playerPrefab, _currentSpawnPoint, Quaternion.identity);
        OnPlayerSpawn?.Invoke(_playerObj);
    }
}
