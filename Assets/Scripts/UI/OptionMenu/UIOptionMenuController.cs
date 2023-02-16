using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOptionMenuController : MonoBehaviour
{
    [Header("View References")]
    [SerializeField]
    private UIOptionMenu optionsMenuPanel;

    public void ShowOptionsMenuPanel()
    {
        if (optionsMenuPanel == null)
            return;

        optionsMenuPanel.Open();
    }

    public void HideOptionsMenuPanel()
    {
        if (optionsMenuPanel == null)
            return;
        Debug.Log("ClosePanel");
        optionsMenuPanel.Close();
    }
}
