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

    private void OnTriggerEnter(Collider other)
    {
        Actor actor = null;
        if(other.gameObject.TryGetComponent(out actor))
        {
            PlayerMovement playerMovement = actor.GetComponent<PlayerMovement>();
            playerMovement.PlayerSpeed = waterPlayerSpeed;
            playerMovement.JumpHeight = waterJumpHeight;

            StartCoroutine(COLateDie(actor));
        }
    }
    private IEnumerator COLateDie(Actor actor)
    {
        yield return new WaitForSeconds(deathDelay);
        actor.Die();
    }
}
