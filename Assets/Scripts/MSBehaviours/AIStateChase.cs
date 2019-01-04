using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIStateChase : LogicalStateMachineBehaviour
{
    private Transform transform;
    private AIController ai;
    private NavMeshAgent navigation;
    private Senses senses;

    private float timeTargetLost;

    protected override void Initialize()
    {
        ai = Animator.GetComponent<AIController>();
        navigation = Animator.GetComponent<NavMeshAgent>();
        senses = Animator.GetComponent<Senses>();
        transform = Animator.transform;
    }

    protected override void Enter()
    {

    }

    protected override void Update()
    {
        if (GameManager.IsPaused)
            return;

        Animator.SetFloat("DistanceToTarget", Vector3.Distance(transform.position, ai.currentTarget.transform.position));

        navigation.SetDestination(ai.currentTarget.transform.position);

        if (!senses.IsVisible(ai.currentTarget))
        {
            timeTargetLost += Time.deltaTime;
        }
        else
            timeTargetLost = 0;

        if (timeTargetLost > 4)
            ai.currentTarget = null;

        if (!ai.currentTarget)
        {
            ExitState();
            return;
        }
    }

    protected override void Exit()
    {
        Animator.SetBool("Chase", false);
    }
}
