using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIStatePatrol : LogicalStateMachineBehaviour
{
    private AIController ai;
    private NavMeshAgent navigation;
    private Rigidbody rigidbody;
    private Senses senses;
    private Transform transform;

    private GameObject player; 

    private Transform[] patrolNodes;
    private int currentIndex;
    private int multiplier;
    private Vector3 idlePosition;

    private bool investigate;

    protected override void Initialize()
    {
        ai = Animator.GetComponent<AIController>();
        navigation = Animator.GetComponent<NavMeshAgent>();
        rigidbody = Animator.GetComponent<Rigidbody>();
        senses = Animator.GetComponent<Senses>();
        transform = Animator.transform;
        player = GameObject.FindGameObjectWithTag("Player");

        EventManager.Subscribe("damage", InvestigateDamage, Animator.gameObject);

        idlePosition = transform.position;
    }

    protected override void Enter()
    {
        patrolNodes = ai.patrolRoute.positions;
        AssignNextNode();
    }

    protected override void Update()
    {
        if (GameManager.IsPaused)
            return;

        if (investigate)
        {
            if (navigation.ReachedDestination())
                investigate = false;        
        }
        else if (ai.patrolRoute)
        {
            if (navigation.ReachedDestination())
            {
                AssignNextNode();
                navigation.destination = patrolNodes[currentIndex].transform.position;
            }
        }
        else
        {
            navigation.SetDestination(idlePosition);
        }

        if (senses.IsVisible(player))
        {
            ai.currentTarget = player;
            Animator.SetBool("Chase", true);
            return;
        }

        rigidbody.velocity = Animator.deltaPosition / Time.deltaTime;
    }

    protected override void Exit()
    {
        navigation.SetDestination(transform.position);
    }

    private void InvestigateDamage(object argument)
    {
        DamageInstance damageInstance = (DamageInstance)argument;

        if (damageInstance == null)
            return;

        investigate = true;

        if (navigation.gameObject)
            navigation.SetDestination(damageInstance.position);
    }

    private void AssignNextNode()
    {
        if (!ai.loopPatrol && (currentIndex >= patrolNodes.Length - 1 || currentIndex <= 0))
        {
            if (currentIndex >= patrolNodes.Length - 1)
                multiplier = -1;
            else if (currentIndex <= 0)
                multiplier = 1;

            currentIndex += multiplier;
        }
        else
        {
            if (currentIndex >= patrolNodes.Length - 1)
                currentIndex = 0;
            else
                currentIndex++;
        }
    }
}
