using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenuController : MonoBehaviour
{
    [Header("View Reference")]
    [SerializeField]
    private UIMainMenu mainMenu;

    [Header("Extra References")]
    [SerializeField]
    private GameObject VFX;

    private void OnEnable()
    {
        mainMenu.OnEnterPressed += OpenMenu;
    }

    private void OnDisable()
    {
        mainMenu.OnEnterPressed -= OpenMenu;
    }

    private void OpenMenu()
    {
        mainMenu.OpenUI();
        VFX.SetActive(true);
    }
}
