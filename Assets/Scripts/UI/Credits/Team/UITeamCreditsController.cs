using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITeamCreditsController : MonoBehaviour
{
    [Header("View References")]
    [SerializeField]
    private UITeamCredits teamCreditsPanel;

    public void ShowTeamCreditsPanel()
    {
        if (teamCreditsPanel == null)
            return;

        teamCreditsPanel.Open();
    }

    public void HideTeamCreditsPanel()
    {
        if (teamCreditsPanel == null)
            return;
        teamCreditsPanel.Close();
    }
}
