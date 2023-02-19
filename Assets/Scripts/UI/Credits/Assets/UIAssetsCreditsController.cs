using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAssetsCreditsController : MonoBehaviour
{
    [Header("View References")]
    [SerializeField]
    private UIAssetsCredits assetsCreditsPanel;

    public void ShowAssetsCreditsPanel()
    {
        if (assetsCreditsPanel == null)
            return;

        assetsCreditsPanel.Open();
    }

    public void HideAssetsCreditsPanel()
    {
        if (assetsCreditsPanel == null)
            return;
        assetsCreditsPanel.Close();
    }
}
