using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [Header("DeathZone Settings")]
    [SerializeField]
    private float deathDelay = 1.0f;

    private void OnTriggerEnter(Collider other)
    {
        Actor actor = null;
        if(other.gameObject.TryGetComponent(out actor))
        {
            actor.GetComponent<PlayerMovement>().PlayerSpeed = 0.6f;
            actor.GetComponent<PlayerMovement>().JumpHeight = 0.4f;
            StartCoroutine(COLateDie(actor));
            
        }
    }
    private IEnumerator COLateDie(Actor actor)
    {
        yield return new WaitForSeconds(deathDelay);
        actor.Die();
    }
}
