using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHighlighter : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private PlayerAim playerAim;

    [Header("Settings")]
    [SerializeField]
    private LayerMask rayLayer;

    private List<Outline> _lastOutlines = new List<Outline>();

    private void Update()
    {
        if(_lastOutlines.Count > 0)
        {
            foreach (Outline outline in _lastOutlines)
            {
                if(outline != null)
                    outline.enabled = false;
            }
                
            _lastOutlines.Clear();
        }

        if (!playerAim.IsAiming)
            return;

        // Create a ray from the camera going through the middle of your screen
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, rayLayer))
        {
            Outline[] outlines = null;

            outlines = hit.collider.gameObject.GetComponentsInChildren<Outline>();
            
            if (outlines.Length <= 0)
                return;

            foreach (Outline outline in outlines)
            {
                outline.enabled = true;
                _lastOutlines.Add(outline);
            }
            return;
        }
        else if (_lastOutlines.Count > 0)
        {
            foreach (Outline outline in _lastOutlines)
                outline.enabled = false;
            _lastOutlines.Clear();
        }
    }
}
