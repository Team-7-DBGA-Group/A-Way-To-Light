using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerVisualCue : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject visualCue;
    [SerializeField]
    private TMP_Text textGui;
   
    public void SetVisualCueActive(bool value)
    {
        visualCue.SetActive(value);
    }

    public void SetVisualText(string text)
    {
        textGui.text = text;
    }

    private void Start()
    {
        visualCue.SetActive(false);
        SetVisualText("F");
    }
}
