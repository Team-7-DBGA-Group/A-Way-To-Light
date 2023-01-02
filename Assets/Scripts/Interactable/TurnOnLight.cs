using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnLight : InteractableObject
{
    [Header("Light Settings")]
    [Tooltip("Valore della luce che si vuole avere")]
    [SerializeField] 
    private float wantedLight = 0;
   
    [Tooltip("Piu' basso e' il valore, piu' veloce si illuminera'")]
    [SerializeField]
    private float lightSpeed = 0;

    private Light _gameLight;
    private float _lightIntensity;

    private void Start()
    {
        _lightIntensity = 0f;

        _gameLight = GetComponent<Light>();
        _gameLight.intensity = _lightIntensity;
    }

    public override void Interact()
    {
        base.Interact();
        TurnOn();
    }

    private void TurnOn()
    {
        StartCoroutine(LightIncrease());
    }

    private IEnumerator LightIncrease()
    {
        for (int ripetizione = 1; ripetizione <= wantedLight; ripetizione++)
        {
            _lightIntensity++;
            _gameLight.intensity = _lightIntensity;
            yield return new WaitForSeconds(lightSpeed);
        }
    }
}


