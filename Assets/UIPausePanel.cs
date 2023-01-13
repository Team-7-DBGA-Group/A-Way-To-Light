using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPausePanel : MonoBehaviour
{
    public bool IsOpen { get; private set; }
    public void Open()
    {
        if (IsOpen)
            return;

        IsOpen = true;
        
    }

    public void Close()
    {
        if (!IsOpen)
            return;

        IsOpen = false;
    }

    private void Start()
    {
        IsOpen = false;
    }
}
