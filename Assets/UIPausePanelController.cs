using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPausePanelController : MonoBehaviour
{
    private UIPausePanel PausePanel;


    private void ShowPausePanel()
    {
        if (PausePanel == null)
            return;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        PausePanel.Open();
    }

    public void HidePausePanel()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        PausePanel.Close();
    }
    
    public void PauseGame()
    {
        GameManager.Instance.PauseGame();
    }

    public void ResumeGame()
    {
        GameManager.Instance.ResumeGame();
    }
}
