using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHeart : MonoBehaviour
{
    public bool IsEmpty { get; private set; }

    [Header("References")]
    [SerializeField]
    private Image fill;
    [SerializeField]
    private Image border;
    [SerializeField]
    private Image background;

    public void SetEnable(bool enable)
    {
        fill.enabled = enable;
        border.enabled = enable;
        background.enabled = enable;
    }

    public void Fill()
    {
        fill.color = new Color32(221, 30, 30, 255);
        IsEmpty = false;
    }

    public void Empty()
    {
        fill.color = new Color32(221, 30, 30, 0);
        IsEmpty = true;
    }

    private void Start()
    {
        Fill();
    }
}
