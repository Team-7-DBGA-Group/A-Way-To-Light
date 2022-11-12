using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAimController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField]
    private Image crosshairImage;

    private void Start()
    {
        HideCrosshair();
    }

    private void OnEnable()
    {
        PlayerAim.OnAimActive += ShowCrosshair;
        PlayerAim.OnAimInactive += HideCrosshair;
    }

    private void OnDisable()
    {
        PlayerAim.OnAimActive -= ShowCrosshair;
        PlayerAim.OnAimInactive -= HideCrosshair;
    }

    private void ShowCrosshair() => crosshairImage.enabled = true;
    private void HideCrosshair() => crosshairImage.enabled = false;
}
