using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class LightShot : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject lightHitPrefab;

    [Header("Light Settings")]
    [SerializeField]
    private float moveSpeed = 2.0f;
    [SerializeField]
    private float secondsToDestroy = 10.0f;

    private Vector3 _moveDirection;
    private bool _canMove = false;

    public void StartMovingToDirection(Vector3 direction)
    {
        if (_canMove)
            return;

        _moveDirection = direction;
        _canMove = true;
        Destroy(gameObject, secondsToDestroy);
    }

    private void Update()
    {
        if (_canMove)
        {
            transform.position += _moveDirection * moveSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactableObj = null;
        if(other.gameObject.TryGetComponent(out interactableObj))
        {
            IInteractable[] interactables = other.gameObject.GetComponents<IInteractable>();

            CustomLog.Log(CustomLog.CustomLogType.GAMEPLAY, "Light Shot interacted with " + other.gameObject.name);
            
            foreach (IInteractable interactable in interactables)
                interactable.Interact();
            
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            Instantiate(lightHitPrefab, hitPoint, Quaternion.identity);

            Destroy(this.gameObject);
        }
        else if(other.gameObject.GetComponentInParent<IInteractable>() != null)
        {
            if (other.gameObject.GetComponent<DialogueTrigger>())
                return;

            IInteractable[] interactables = other.gameObject.GetComponentsInParent<IInteractable>();

            CustomLog.Log(CustomLog.CustomLogType.GAMEPLAY, "Light Shot interacted with " + other.gameObject.name);

            foreach (IInteractable interactable in interactables)
                interactable.Interact();

            Vector3 hitPoint = other.ClosestPoint(transform.position);
            Instantiate(lightHitPrefab, hitPoint, Quaternion.identity);

            Destroy(this.gameObject);
        }
        else if(other != null && !other.gameObject.tag.Equals("Player") && other != this.gameObject.GetComponent<Collider>())
        {
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            Instantiate(lightHitPrefab, hitPoint, Quaternion.identity);
            
            Destroy(this.gameObject);
        }
    }
}
