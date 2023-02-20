using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILocationPanelController : MonoBehaviour
{
    [Header("View References")]
    [SerializeField]
    private UILocationPanel locationPanel;

    private void OnEnable()
    {
        SpawnManager.OnPlayerReady += ShowLocationPanel;
    }

    private void OnDisable()
    {
        SpawnManager.OnPlayerReady -= ShowLocationPanel;
    }

    public void ShowLocationPanel()
    {
        if (locationPanel == null)
            return;

        locationPanel.Open();
    }
}
