using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameOverPanelController : MonoBehaviour
{
    [Header("View References")]
    [SerializeField]
    private UIGameOverPanel gameOverPanel;
   
    public void HidGameOverPanel()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        gameOverPanel.Close();
    }

    private void OnEnable()
    {
        Player.OnPlayerDie += ShowGameOverPanel;
        UIGameOverPanel.OnPanelClose += CallSceneReset;
    }

    private void OnDisable()
    {
        Player.OnPlayerDie -= ShowGameOverPanel;
        UIGameOverPanel.OnPanelClose -= CallSceneReset;
    }

    private void ShowGameOverPanel()
    {
        if (gameOverPanel == null)
            return;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gameOverPanel.Open();
    }

    private void CallSceneReset()
    {
        // Call Reset Method from Game Manager
        Debug.Log("Reset HERE");
    }
}
