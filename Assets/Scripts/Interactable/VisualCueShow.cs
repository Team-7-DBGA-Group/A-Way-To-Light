using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualCueShow : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private string keyToShow;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerVisualCue pVisualCue))
        {
            pVisualCue.SetVisualCueActive(true);
            pVisualCue.SetVisualText(keyToShow);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerVisualCue pVisualCue))
            pVisualCue.SetVisualCueActive(false);
    }
}
