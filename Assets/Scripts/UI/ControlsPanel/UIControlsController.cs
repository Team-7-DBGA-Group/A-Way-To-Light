using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControlsController : MonoBehaviour
{
    [Header("View References")]
    [SerializeField]
    private UIControls controlsPanel;

    public void ShowControlsPanel()
    {
        if (controlsPanel == null)
            return;

        controlsPanel.Open();
    }

    public void HideControlsPanel()
    {
        if (controlsPanel == null)
            return;
        controlsPanel.Close();
    }
}
