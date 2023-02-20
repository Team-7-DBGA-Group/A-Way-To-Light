using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHintPanelController : MonoBehaviour
{
    
    [Header("View References")]
    [SerializeField]
    private UIHintPanelMenu hintPanel;

    private void OnEnable()
    {
        SpawnManager.OnPlayerReady += ShowHintPanel; 
        GameManager.OnPauseAction += ShowHintPanel;
    }

    private void OnDisable()
    {
        SpawnManager.OnPlayerReady -= ShowHintPanel;
        GameManager.OnPauseAction -= ShowHintPanel;
    }

    public void ShowHintPanel()
    {
        if (hintPanel == null)
            return;

        hintPanel.Open();
    }

    //public void HideHintPanel()
    //{
    //    if (hintPanel == null)
    //        return;
    //    hintPanel.Close();
    //}
}
