using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TransportableObject : MonoBehaviour
{
    [SerializeField]
    private bool _onTransport = false;
    // Start is called before the first frame update
    // LUCA TI MOLLO L'OSSO NON RICONOSCE LA COLLISIONE CON LA BARCA, LE HO PROVATE TUTTE
    private void Update()
    {
        if(_onTransport)
        {
            //_player.transform.SetParent(gameObject.transform);
        }
        
        else
        {
           // _player.transform.SetParent(null);
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        _onTransport = true;
        
    }

    private void OnCollisionExit(Collision collision)
    {
        _onTransport = false;
        
    }



}
