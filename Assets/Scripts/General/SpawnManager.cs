using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnManager : Singleton<SpawnManager>, IDataPersistence
{
    public static event Action<GameObject> OnPlayerSpawn;

    [Header("References")]
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private Transform startSpawnPoint;

    private Vector3 _currentSpawnPoint = Vector3.zero;
    private GameObject _playerObj = null;

    public void LoadData(GameData data)
    {
        _currentSpawnPoint = data.PlayerSpawnPosition;
    }

    public void SaveData(GameData data)
    {
        data.PlayerSpawnPosition = _currentSpawnPoint;
    }

    public void SetNewSpawnPoint(Vector3 position)
    {
        _currentSpawnPoint = position;
    }

    public void SpawnPlayer()
    {
        _playerObj = Instantiate(playerPrefab, _currentSpawnPoint, Quaternion.identity);
        OnPlayerSpawn?.Invoke(_playerObj);
    }

    private void Start()
    {
        _currentSpawnPoint = startSpawnPoint.transform.position;
        SpawnPlayer();
    }
}
