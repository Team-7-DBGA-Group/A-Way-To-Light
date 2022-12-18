using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomisationManager : Singleton<CustomisationManager>
{
    [Header("Parts")]
    [SerializeField]
    private List<GameObject> bodies;
    [SerializeField]
    private List<GameObject> hair;
    [SerializeField]
    private List<GameObject> hats;
    [SerializeField]
    private List<GameObject> heads;
    [SerializeField]
    private List<GameObject> leftArms;
    [SerializeField]
    private List<GameObject> rightArms;

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
    private GameObject playerRefPrefab;

    private GameObject _currentBody;
    private GameObject _currentHair;
    private GameObject _currentHat;
    private GameObject _currentHead;
    private GameObject _currentLeftArm;
    private GameObject _currentRightArm;

    private int _headIndex = 0;
    private int _hairIndex = 0;
    private int _bodyIndex = 0;
    private int _armsIndex = 0;
    private int _hatIndex = 0;

    //public void LoadCharacter(int headIndex, int hairIndex, int bodyIndex, int armsIndex, int hatIndex)
    //{
    //    _currentHead = heads[headIndex];
    //    _currentHair = hair[hairIndex];
    //    _currentBody = bodies[bodyIndex];
    //    _currentLeftArm = leftArms[headIndex];
    //    _currentBody = heads[headIndex];
    //}

    public void SetPlayerReferences(GameObject bodyRef, GameObject hairRef, GameObject headRef, GameObject lArmRef, GameObject rArmRef)
    {
        bodyReference = bodyRef;
        hairReference = hairRef;
        headReference = headRef;
        leftArmReference = lArmRef;
        rightArmReference = rArmRef;
    }

    public void NextHead()
    {
        _headIndex = (_headIndex + 1) % heads.Count;
        _currentHead = heads[_headIndex];
        CreateCharacter();
    }

    public void NextHair()
    {
        _hairIndex = (_hairIndex + 1) % hair.Count;
        _currentHair = hair[_hairIndex];
        CreateCharacter();
    }

    public void NextBody()
    {
        _bodyIndex = (_bodyIndex + 1) % bodies.Count;
        _currentBody = bodies[_bodyIndex];
        CreateCharacter();
    }

    public void NextArms()
    {
        _armsIndex = (_armsIndex + 1) % leftArms.Count;
        _currentLeftArm = leftArms[_armsIndex];
        _currentRightArm = rightArms[_armsIndex];
        CreateCharacter();
    }

    public void NextHat()
    {
        _hatIndex = (_hatIndex + 1) % hats.Count;
        _currentHat.SetActive(false);
        _currentHat = hats[_hatIndex];
        CreateCharacter();
    }

    public void PreviousHead()
    {
        _headIndex = (_headIndex - 1) % heads.Count;
        _currentHead = heads[_headIndex];
        CreateCharacter();
    }

    private void Start()
    {
        _currentBody = bodies[0];
        _currentHair = hair[0];
        _currentHat = hats[0];
        _currentHead = heads[0];
        _currentLeftArm = leftArms[0];
        _currentRightArm = rightArms[0];
        CreateCharacter();
    }

    private void RandomCharacter()
    {
        _currentBody = bodies[Random.Range(0, bodies.Count)];
        _currentHair = hair[Random.Range(0, hair.Count)];
        _currentHat.SetActive(false);
        _currentHat = hats[Random.Range(0, hats.Count)];
        _currentHead = heads[Random.Range(0, heads.Count)];
        int armsIndex = Random.Range(0, leftArms.Count);
        _currentLeftArm = leftArms[armsIndex];
        _currentRightArm = rightArms[armsIndex];
    }

    private void CreateCharacter()
    {
        bodyReference.GetComponent<MeshRenderer>().sharedMaterials = _currentBody.GetComponent<MeshRenderer>().sharedMaterials;
        bodyReference.GetComponent<MeshFilter>().sharedMesh = _currentBody.GetComponent<MeshFilter>().sharedMesh;

        headReference.GetComponent<MeshRenderer>().sharedMaterials = _currentHead.GetComponent<MeshRenderer>().sharedMaterials;
        headReference.GetComponent<MeshFilter>().sharedMesh = _currentHead.GetComponent<MeshFilter>().sharedMesh;

        _currentHat.SetActive(true);

        if (_currentHat.GetComponent<CustomHat>().canHair)
        {
            hairReference.GetComponent<MeshRenderer>().sharedMaterials = _currentHair.GetComponent<MeshRenderer>().sharedMaterials;
            hairReference.GetComponent<MeshFilter>().sharedMesh = _currentHair.GetComponent<MeshFilter>().sharedMesh;
        }
        else
        {
            hairReference.GetComponent<MeshRenderer>().sharedMaterials = hair[hair.Count - 1].GetComponent<MeshRenderer>().sharedMaterials;
            hairReference.GetComponent<MeshFilter>().sharedMesh = hair[hair.Count - 1].GetComponent<MeshFilter>().sharedMesh;
        }

        leftArmReference.GetComponent<MeshRenderer>().sharedMaterials = _currentLeftArm.GetComponent<MeshRenderer>().sharedMaterials;
        leftArmReference.GetComponent<MeshFilter>().sharedMesh = _currentLeftArm.GetComponent<MeshFilter>().sharedMesh; 
        rightArmReference.GetComponent<MeshRenderer>().sharedMaterials = _currentRightArm.GetComponent<MeshRenderer>().sharedMaterials;
        rightArmReference.GetComponent<MeshFilter>().sharedMesh = _currentRightArm.GetComponent<MeshFilter>().sharedMesh;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RandomCharacter();
            CreateCharacter();
        }
    }

}
