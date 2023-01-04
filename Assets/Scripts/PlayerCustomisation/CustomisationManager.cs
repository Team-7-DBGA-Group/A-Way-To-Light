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

    [SerializeField]
    private GameObject playerRefPrefab;
    
    PlayerCustomisation _playerCustom;

    private GameObject _currentBody;
    private GameObject _currentHair;
    private GameObject _currentHat;
    private GameObject _currentHead;
    private GameObject _currentLeftArm;
    private GameObject _currentRightArm;

    private GameObject _bodyReference;
    private GameObject _hairReference;
    private GameObject _headReference;
    private GameObject _leftArmReference;
    private GameObject _rightArmReference;

    private int _headIndex = 0;
    private int _hairIndex = 0;
    private int _bodyIndex = 0;
    private int _armsIndex = 0;
    private int _hatIndex = 0;

    public void LoadCharacter(int headIndex, int hairIndex, int bodyIndex, int armsIndex, int hatIndex)
    {
        _currentHead = heads[headIndex];
        _currentHair = hair[hairIndex];
        _currentBody = bodies[bodyIndex];
        _currentLeftArm = leftArms[armsIndex];
        _currentRightArm = rightArms[armsIndex];
        _currentHat = hats[hatIndex];
        CreateCharacter();
    }

    public void SetPlayerReferences(GameObject bodyRef, GameObject hairRef, GameObject headRef, GameObject lArmRef, GameObject rArmRef)
    {
        _bodyReference = bodyRef;
        _hairReference = hairRef;
        _headReference = headRef;
        _leftArmReference = lArmRef;
        _rightArmReference = rArmRef;
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
        _playerCustom = playerRefPrefab.GetComponent<PlayerCustomisation>();
        _currentBody = bodies[_playerCustom.BodyIndex];
        _currentHair = hair[_playerCustom.HairIndex];
        _currentHat = hats[_playerCustom.HatIndex];
        _currentHead = heads[_playerCustom.HeadIndex];
        _currentLeftArm = leftArms[_playerCustom.ArmsIndex];
        _currentRightArm = rightArms[_playerCustom.ArmsIndex];
        CreateCharacter();
    }

    private void RandomCharacter()
    {
        _currentBody = bodies[_bodyIndex = Random.Range(0, bodies.Count)];
        _currentHair = hair[_hairIndex = Random.Range(0, hair.Count)];
        _currentHat.SetActive(false);
        _currentHat = hats[_hatIndex = Random.Range(0, hats.Count)];
        _currentHead = heads[_headIndex = Random.Range(0, heads.Count)];
        _armsIndex = Random.Range(0, leftArms.Count);
        _currentLeftArm = leftArms[_armsIndex];
        _currentRightArm = rightArms[_armsIndex];
    }

    private void CreateCharacter()
    {
        _bodyReference.GetComponent<MeshRenderer>().sharedMaterials = _currentBody.GetComponent<MeshRenderer>().sharedMaterials;
        _bodyReference.GetComponent<MeshFilter>().sharedMesh = _currentBody.GetComponent<MeshFilter>().sharedMesh;

        _headReference.GetComponent<MeshRenderer>().sharedMaterials = _currentHead.GetComponent<MeshRenderer>().sharedMaterials;
        _headReference.GetComponent<MeshFilter>().sharedMesh = _currentHead.GetComponent<MeshFilter>().sharedMesh;

        _currentHat.SetActive(true);

        if (_currentHat.GetComponent<CustomHat>().canHair)
        {
            _hairReference.GetComponent<MeshRenderer>().sharedMaterials = _currentHair.GetComponent<MeshRenderer>().sharedMaterials;
            _hairReference.GetComponent<MeshFilter>().sharedMesh = _currentHair.GetComponent<MeshFilter>().sharedMesh;
        }
        else
        {
            _hairReference.GetComponent<MeshRenderer>().sharedMaterials = hair[hair.Count - 1].GetComponent<MeshRenderer>().sharedMaterials;
            _hairReference.GetComponent<MeshFilter>().sharedMesh = hair[hair.Count - 1].GetComponent<MeshFilter>().sharedMesh;
        }

        _leftArmReference.GetComponent<MeshRenderer>().sharedMaterials = _currentLeftArm.GetComponent<MeshRenderer>().sharedMaterials;
        _leftArmReference.GetComponent<MeshFilter>().sharedMesh = _currentLeftArm.GetComponent<MeshFilter>().sharedMesh; 
        _rightArmReference.GetComponent<MeshRenderer>().sharedMaterials = _currentRightArm.GetComponent<MeshRenderer>().sharedMaterials;
        _rightArmReference.GetComponent<MeshFilter>().sharedMesh = _currentRightArm.GetComponent<MeshFilter>().sharedMesh;

        _playerCustom.SetData(_headIndex, _hairIndex, _bodyIndex, _armsIndex, _hatIndex);
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
