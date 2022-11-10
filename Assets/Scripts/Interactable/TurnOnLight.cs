using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnLight : MonoBehaviour, Iinteractable
{
    public float LightIntensity;
    Light GameLight;
    private void Start()
    {

        GameLight = GetComponent<Light>();
    }
    public void TurnOn()
    {
        GameLight.intensity = LightIntensity;
    }

    public void Interact()
    {
        TurnOn();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.K))
        {
            Interact();
        }

    }
}


