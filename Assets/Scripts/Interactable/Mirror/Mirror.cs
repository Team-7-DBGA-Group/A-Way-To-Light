using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : InteractableObject
{
    [Header("References")]
    [SerializeField]
    private Transform shotPoint;
    [SerializeField]
    private GameObject lightShotPrefab;
    [SerializeField]
    private GameObject rotatingPoint;

    [Header("Settings")]
    [SerializeField]
    private float reflactionDelay = 0.1f;

    private float _currentRotation = 0;
    private bool _canInteract = false;

    public override void Interact()
    {
        StartCoroutine(COReflectShot());
    }

    private void Start()
    {
        rotatingPoint.transform.Rotate(new Vector3(0, _currentRotation, 0));
    }

    private void Update()
    {
        if (!_canInteract)
            return;

        if (Input.GetKeyDown(KeyCode.F))
            NextRotation();
    }

    private void NextRotation()
    {
        _currentRotation += 90;
        if (_currentRotation > 360)
            _currentRotation = 0;
        rotatingPoint.transform.eulerAngles = new Vector3(0, _currentRotation, 0);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            _canInteract = true;
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            _canInteract = false;
        }
    }

    IEnumerator COReflectShot()
    {
        yield return new WaitForSeconds(reflactionDelay);

        GameObject shot = Instantiate(lightShotPrefab, shotPoint.position, Quaternion.identity);
        shot.GetComponent<LightShot>().StartMovingToDirection(shotPoint.forward);
    }
}
