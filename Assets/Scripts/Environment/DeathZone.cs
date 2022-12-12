using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [Header("DeathZone Settings")]
    [SerializeField]
    private float deathDelay = 1.0f;
    [SerializeField]
    private float waterPlayerSpeed = 0.6f;
    [SerializeField]
    private float waterJumpHeight = 0.4f;

    private bool _isDying = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_isDying)
            return;

        Actor actor = null;
        actor = other.GetComponentInParent<Actor>();
        if(actor != null || other.gameObject.TryGetComponent(out actor))
        {
            if (actor is Player)
            {
                PlayerMovement playerMovement = actor.GetComponent<PlayerMovement>();
                playerMovement.PlayerSpeed = waterPlayerSpeed;
                playerMovement.JumpHeight = waterJumpHeight;
            }
            
            StartCoroutine(COLateDie(actor));
        }
    }
    private IEnumerator COLateDie(Actor actor)
    {
        _isDying = true;
        yield return new WaitForSeconds(deathDelay);
        actor.Die();
        _isDying = false;
    }
}
