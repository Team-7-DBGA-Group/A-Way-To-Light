using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoadingIcon : MonoBehaviour
{
    public void SetActiveIcon(bool active)
    {
        this.gameObject.SetActive(active);
    }

    private void Awake()
    {
        this.gameObject.SetActive(false);
    }
}
