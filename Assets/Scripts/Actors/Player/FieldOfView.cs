using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public bool CanSeeActor { get; private set; }
    public float Radius { get => radius; }
    public float Angle { get => angle; }
    public GameObject ActorToFollow { get => _actorToFollow; }

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

    private GameObject _actorToFollow;
    private Coroutine _COAim;

    private void OnEnable()
    {
        EnemyManager.OnCombatEnter += OnStartAim;
        EnemyManager.OnCombatExit += OnStopAim;
    }

    private void OnDisable()
    {
        EnemyManager.OnCombatEnter -= OnStartAim;
        EnemyManager.OnCombatExit -= OnStopAim;
    }

    private void OnStartAim()
    {
        _COAim = StartCoroutine(COFovRoutine(searchDelay));
    }

    private void OnStopAim()
    {
        if(_COAim != null)
            StopCoroutine(_COAim);
    }

    private void Start()
    {
        CanSeeActor = false;    
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
        

        if (rangeChecks.Length != 0)
        {
            for(int i = 0; i < rangeChecks.Length; i++)
            {
                Transform target = rangeChecks[i].transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
                {
                    _actorToFollow = rangeChecks[i].gameObject;
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstaclesMask))
                    {
                        NPC npc = _actorToFollow.GetComponentInParent(typeof(NPC)) as NPC;
                        if(npc != null && npc.IsAlive)
                            CanSeeActor = true;
                    }
                    else
                    {
                        _actorToFollow = null;
                        CanSeeActor = false;
                    }
                    break;
                }
                else
                {
                    _actorToFollow = null;
                    CanSeeActor = false;
                }
            }
        }
        else if (CanSeeActor)
        {
            _actorToFollow = null;
            CanSeeActor = false;
        }
    }
}
