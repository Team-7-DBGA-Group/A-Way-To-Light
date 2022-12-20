using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomPlayerData", menuName = "ScriptableObjects/CustomPlayerData")]
public class CustomisationData : ScriptableObject
{
    public int HeadIndex { get; private set; }
    public int HairIndex { get; private set; }
    public int BodyIndex { get; private set; }
    public int ArmsIndex { get; private set; }
    public int HatIndex { get; private set; }

    public void SetData(int headIndex, int hairIndex, int bodyIndex, int armsIndex, int hatIndex)
    {
        HeadIndex = headIndex;
        HairIndex = hairIndex;
        BodyIndex = bodyIndex;
        ArmsIndex = armsIndex;
        HatIndex = hatIndex;
    }

    public void LoadCustomPlayer()
    {
        CustomisationManager.Instance.LoadCharacter(HeadIndex, HairIndex, BodyIndex, ArmsIndex, HatIndex);
    }
}
