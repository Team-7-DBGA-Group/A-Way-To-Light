using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomHead : MonoBehaviour
{
    [SerializeField]
    private bool hair = false;

    public bool hasHair { get => hair; }
}
