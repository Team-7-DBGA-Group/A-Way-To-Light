using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    // They can't be Properties because of Serialization's rules
    public Vector3 PlayerSpawnPosition;
    public bool IsStartingCutscenePlayed;
    public SerializableDictionary<string, bool> InteractablesActivated;
    public SerializableDictionary<string, bool> AutoDialoguesActivated;
    public SerializableDictionary<string, bool> BasicEnemiesAlive;
    public SerializableDictionary<string, bool> BasicEnemiesDead;
    public SerializableDictionary<string, bool> AlliesAlive;
    public List<int> CustomizationIndexes;

    // Default values when the game start
    public GameData(Vector3 playerStartingPosition)
    {
        this.PlayerSpawnPosition = playerStartingPosition;
        this.IsStartingCutscenePlayed = false;
        InteractablesActivated = new SerializableDictionary<string, bool>();
        AutoDialoguesActivated = new SerializableDictionary<string, bool>();
        BasicEnemiesAlive = new SerializableDictionary<string, bool>();
        BasicEnemiesDead = new SerializableDictionary<string, bool>();
        AlliesAlive = new SerializableDictionary<string, bool>();
        CustomizationIndexes = new List<int>() { 0, 0, 0, 0, 0};
    }
}
