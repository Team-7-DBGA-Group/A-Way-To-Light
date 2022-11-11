using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimController : MonoBehaviour
{
    [SerializeField]
    private GameObject PlayerCamera;
    [SerializeField]
    private GameObject AimCamera;


    private void Start()
    {
        PlayerCamera.SetActive(true);
        AimCamera.SetActive(false);
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            PlayerCamera.SetActive(false);
            AimCamera.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            PlayerCamera.SetActive(true);
            AimCamera.SetActive(false);
        }
    }
}
