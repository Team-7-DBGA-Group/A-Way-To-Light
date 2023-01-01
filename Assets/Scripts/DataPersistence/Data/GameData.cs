using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    // They can't be Properties because of Serialization's rules
    public Vector3 PlayerSpawnPosition;
    public bool IsStartingCutscenePlayed;

    // Default values when the game start
    public GameData(Vector3 playerStartingPosition)
    {
        this.PlayerSpawnPosition = playerStartingPosition;
        this.IsStartingCutscenePlayed = false;
    }
}
