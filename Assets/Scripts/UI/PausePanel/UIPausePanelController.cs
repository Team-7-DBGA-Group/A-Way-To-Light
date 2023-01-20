using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class UIPausePanelController : MonoBehaviour
{
    [Header("View References")]
    [SerializeField]
    private UIPausePanel pausePanel;
    
    private void OnEnable()
    {
        GameManager.OnPause += UpdateUI;
    }

    private void OnDisable()
    {
        GameManager.OnPause -= UpdateUI;
    }

    private void UpdateUI(bool isShow)
    {
        if (isShow)
            ShowPausePanel();
        else
            HidePausePanel();
    }

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }

    private void ShowPausePanel()
    {
        if (pausePanel == null)
            return;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        pausePanel.Open();
    }

    public void HidePausePanel()
    {
        if (pausePanel == null)
            return;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pausePanel.Close();
    }
}
