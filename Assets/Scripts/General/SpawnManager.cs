using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnManager : Singleton<SpawnManager>, IDataPersistence
{
    public static event Action<GameObject> OnPlayerSpawn;

    public GameObject PlayerObj { get => _playerObj; }

    public Vector3 StartingSpawnPoint { get => startSpawnPoint.transform.position; }

    [Header("References")]
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private Transform startSpawnPoint;

    [Header("Settings")]
    [SerializeField]
    private bool forceSpawnPosition = false;

    private Vector3 _currentSpawnPoint = Vector3.zero;
    private GameObject _playerObj = null;
    private bool _shouldUseStartingPos = true;

    public void LoadData(GameData data)
    {
        _shouldUseStartingPos = data.ShouldUseStartingPos;

        if (forceSpawnPosition)
        {
            _currentSpawnPoint = StartingSpawnPoint;
            return;
        }
       
        if (_shouldUseStartingPos)
        {
            _currentSpawnPoint = StartingSpawnPoint;
        }
        else
        {
            _currentSpawnPoint = data.PlayerSpawnPosition;
        }
        Debug.Log("should spawn start: " + _shouldUseStartingPos);
        Debug.Log("SpawnPoint: " + _currentSpawnPoint);
    }

    public void SaveData(GameData data)
    {
        data.PlayerSpawnPosition = _currentSpawnPoint;
        data.ShouldUseStartingPos = _shouldUseStartingPos;
    }

    public void SetNewSpawnPoint(Vector3 position)
    {
        Debug.Log("SpawnPoint set to " + position);
        _currentSpawnPoint = position;
        _shouldUseStartingPos = false;
    }

    public void ResetManager()
    {
        _shouldUseStartingPos = true;
    }

    public void SpawnPlayer()
    {
        _playerObj = Instantiate(playerPrefab, _currentSpawnPoint, Quaternion.identity);
        OnPlayerSpawn?.Invoke(_playerObj);
    }

    private void Start()
    {
        if (DataPersistenceManager.Instance.HasData)
            SpawnPlayer();
    }
}
