using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float AttackRange { get => attackRange; }

    [Header("Enemy Settings")]
    [SerializeField]
    private float attackRange = 1.0f;

    // State Machine
    protected FSMSystem FSM;
}
