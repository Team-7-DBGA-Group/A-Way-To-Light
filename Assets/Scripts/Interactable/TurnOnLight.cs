using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnLight : MonoBehaviour, Iinteractable
{
    [Header("Valore della luce da impostare")]
    [SerializeField] private float WantedLight = 0;
    private float LightIntensity;
    [Header("Piu' basso e' il valore,piu' veloce si illuminera'")]
    [SerializeField]private float LightSpeed = 0;
    Light GameLight;
    
    
    private void Start()
    {
        LightIntensity = 0f;
        GameLight = GetComponent<Light>();
        
    }
    public void TurnOn()
    {
        StartCoroutine(LightIncrease());
    }

    public void Interact()
    {
        TurnOn();
    }
    
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            Interact();
        }
    }
    IEnumerator LightIncrease()
    {
        for (int ripetizione = 1; ripetizione <= WantedLight; ripetizione++)
        {
            LightIntensity++;
            GameLight.intensity = LightIntensity;
            yield return new WaitForSeconds(LightSpeed);
        }
        

    }
}


