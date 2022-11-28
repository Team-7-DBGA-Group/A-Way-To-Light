using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public bool CanSeeActor { get; private set; }
    public float Radius { get => radius; }
    public float Angle { get => angle; }
    public GameObject ActorToFollow { get => actorToFollow; }

    [Header("FOV Settings")]
    [SerializeField]
    private float radius = 0f;
    [Range(0,360)]
    [SerializeField]
    private float angle = 0f;
    [SerializeField]
    private LayerMask targetMask;
    [SerializeField]
    private LayerMask obstaclesMask;
    [SerializeField]
    private float searchDelay = 0.2f;

    [Header("References")]
    [SerializeField]
    private GameObject actorToFollow;

    //private GameObject[] actors;

    private void Start()
    {
        StartCoroutine(COFovRoutine(searchDelay));    
    }

    private IEnumerator COFovRoutine(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FOVCheck();
        }
    }

    private void FOVCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);
        //for(int i = 0; i < rangeChecks.Length; i++)
        //{

        //}

        if (rangeChecks.Length != 0)
        {
            for(int i = 0; i < rangeChecks.Length; i++)
            {
                Transform target = rangeChecks[i].transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
                {
                    
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstaclesMask))
                        CanSeeActor = true;
                    else
                        CanSeeActor = false;
                }
                else
                    CanSeeActor = false;
            }
        }
        else if (CanSeeActor)
            CanSeeActor = false;
    }
}
