using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Player : Actor
{
    public override void Die()
    {
        CustomLog.Log(CustomLog.CustomLogType.GAMEPLAY, "Game Over!");
        Destroy(gameObject);
    }
}
