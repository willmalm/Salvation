using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Combat : MonoBehaviour
{
    bool canAttack = true;
    NavMeshAgent navAgent;
    Animator animator;
    public Collider weaponCollider;
    Condition condition;

    private GameObject _target;

    public GameObject CurrentTarget
    {
        get
        {
            return _target;
        }
    }

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        condition = GetComponent<Condition>();
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public void StartAttack()
    {
        navAgent.isStopped = true;
        canAttack = false;
    }

    public void ActivateTrigger()
    {
        weaponCollider.enabled = true;
        //condition.
    }

    public void DeactivateTrigger()
    {
        weaponCollider.enabled = false;
        canAttack = true;
    }

    public void EndRecovery()
    {
        //canRotate = true;
        navAgent.isStopped = false;
    }
}
