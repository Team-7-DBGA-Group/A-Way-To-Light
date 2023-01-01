using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    // They can't be Properties because of Serialization's rules
    public Vector3 PlayerSpawnPosition;

    // Default values when the game start
    public GameData()
    {
        // this should be the starting position of the player given by the transform, mmh let's see
        // we could pass it as arg.
        this.PlayerSpawnPosition = Vector3.zero;
    }
}
