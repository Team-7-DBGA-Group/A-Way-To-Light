using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIWeaponIcon : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Image iconImage;
    [SerializeField]
    private Sprite nullIcon;

    public void ResetIcon() => iconImage.sprite = nullIcon;

    public void SetIcon(Sprite icon) => iconImage.sprite = icon;
}
