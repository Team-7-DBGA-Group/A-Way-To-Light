using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UILightCharge : MonoBehaviour
{
    public bool IsOn { get; private set; }

    [Header("References")]
    [SerializeField]
    private Image iconImage = null;

    [Header("UI Settings")]
    [SerializeField]
    private Color32 disabledColor = Color.gray;
    [SerializeField]
    private Color32 blinkColor = Color.yellow;
    [SerializeField]
    private float blinkSpeed = 0.2f;
    [SerializeField]
    private int blinkNumber = 2;

    private Color32 _defaultColor = Color.white;
    private bool _isBlinking = false;

    public void Off()
    {
        if (!IsOn)
            return;

        StopAllCoroutines();
        iconImage.color = disabledColor;
        IsOn = false;
    }

    public void On()
    {
        /*if (_isBlinking)
            return;*/

        if (IsOn)
            return;

        IsOn = true;
        StartCoroutine(COBlinkAnimation());
    }

    private void Start()
    {
        iconImage.color = _defaultColor;
        IsOn = true;
    }

    private IEnumerator COBlinkAnimation()
    {
        _isBlinking = true;

        iconImage.color = _defaultColor;

        for(int i = 0; i < blinkNumber; ++i)
        {
            iconImage.color = _defaultColor;
            yield return new WaitForSeconds(blinkSpeed);
            iconImage.color = blinkColor;
            yield return new WaitForSeconds(blinkSpeed);
        }

        iconImage.color = _defaultColor;

        _isBlinking = false;
    }
}
