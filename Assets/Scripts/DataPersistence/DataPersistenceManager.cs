using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using System.Linq;

public class DataPersistenceManager : Singleton<DataPersistenceManager>
{
    [Header("File Storage Config")]
    [SerializeField]
    private string fileName = "data.light";

    private GameData _gameData;
    private List<IDataPersistence> _dataPersistenceObjects;

    // If you want other handlers, it's better to create an abstraction DataHandler
    // and a method for setting it.
    private FileDataHandler _fileDataHandler;

    public void NewGame()
    {
        _gameData = new GameData();
    }

    public void LoadGame()
    {
        // Load any saved data from a a file using the data handler
        _gameData = _fileDataHandler.Load();
        
        // If no data can be loaded, initialize to a new game
        if(_gameData == null)
        {
            CustomLog.Log(CustomLog.CustomLogType.SYSTEM, "No data was found. Initializing data to defaults.");
            NewGame();
        }
        
        // Push the loaded data to all other scripts that need it
        foreach(IDataPersistence dataPersistenceObj in _dataPersistenceObjects)
            dataPersistenceObj.LoadData(_gameData);

        CustomLog.Log(CustomLog.CustomLogType.SYSTEM, "Game Loaded");
    }

    public void SaveGame()
    {
        // pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects)
            dataPersistenceObj.SaveData(_gameData);

        // Save that data to a file using the data handler
        _fileDataHandler.Save(_gameData);

        CustomLog.Log(CustomLog.CustomLogType.SYSTEM, "Game Saved");
    }

    private void Start()
    {
        _fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        _dataPersistenceObjects = FindAllDataPersistenceObjects();
        // I dunno, maybe problems which Start() begins first
        LoadGame(); 
    }

    private void OnApplicationQuit() => SaveGame();

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
