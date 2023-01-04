using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCustomisation : MonoBehaviour, IDataPersistence
{
    public int HeadIndex { get; private set; }
    public int HairIndex { get; private set; }
    public int BodyIndex { get; private set; }
    public int ArmsIndex { get; private set; }
    public int HatIndex { get; private set; }

    [Header("References")]
    [SerializeField]
    private GameObject bodyReference;
    [SerializeField]
    private GameObject hairReference;
    [SerializeField]
    private GameObject headReference;
    [SerializeField]
    private GameObject leftArmReference;
    [SerializeField]
    private GameObject rightArmReference;
    [SerializeField]
    private GameObject hatReference;

    public void LoadData(GameData data)
    {
        List<int> indixes = data.CustomizationIndixes;
        HeadIndex = indixes[0];
        HairIndex = indixes[1];
        BodyIndex = indixes[2];
        ArmsIndex = indixes[3];
        HatIndex = indixes[4];
    }

    public void SaveData(GameData data)
    {
        List<int> indixes = new List<int>() { HeadIndex, HairIndex, BodyIndex, ArmsIndex, HatIndex };
        data.CustomizationIndixes = indixes;
    }

    public void SetData(int headIndex, int hairIndex, int bodyIndex, int armsIndex, int hatIndex)
    {
        HeadIndex = headIndex;
        HairIndex = hairIndex;
        BodyIndex = bodyIndex;
        ArmsIndex = armsIndex;
        HatIndex = hatIndex;
    }

    public void LoadCustomisation()
    {
        CustomisationManager.Instance.LoadCharacter(HeadIndex, HairIndex, BodyIndex, ArmsIndex, HatIndex);
    }

    private void Start()
    {
        CustomisationManager.Instance.SetPlayerReferences(bodyReference, hairReference, headReference, leftArmReference, rightArmReference, hatReference);
        LoadCustomisation();
    }
}
