using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StaffKey : GateKey
{
    [Header("References")]
    [SerializeField]
    private GameObject deactivatingObject;

    [Header("Materials settings")]
    [SerializeField]
    private Material offMaterial;
    [SerializeField]
    private Material onMaterial;

    [Header("Sounds")]
    [SerializeField]
    private AudioClip onDeactivatingSound;
    
    private MeshRenderer _meshRenderer;
    private bool _soundPlayed = false;

    protected override void CustomInteraction()
    {
        GameObject destroyObject = deactivatingObject;
        _meshRenderer.material = onMaterial;
        if (!_soundPlayed)
        {
            AudioManager.Instance.PlaySound(onDeactivatingSound);
            _soundPlayed = true;
        }
        Destroy(destroyObject);
    }

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.material = offMaterial;
    }

}
