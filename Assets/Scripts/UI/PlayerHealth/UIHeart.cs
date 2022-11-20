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
        fill.enabled = true;
        IsEmpty = true;
    }

    public void Empty()
    {
        fill.enabled = false;
        IsEmpty = false;
    }

    private void Start()
    {
        Fill();
    }
}
