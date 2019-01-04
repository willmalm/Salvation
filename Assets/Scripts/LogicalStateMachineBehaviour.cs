using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicalStateMachineBehaviour : StateMachineBehaviour
{
    private bool active;

    protected Animator Animator { get; private set; }

    public bool enterImmediately = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Animator = animator;
        Initialize();

        active = false;


        if (!enterImmediately)
            return;

        Enter();
        active = true;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (active)
        {
            Update();
        }
        else if (!enterImmediately && animator.GetAnimatorTransitionInfo(layerIndex).normalizedTime > 0)
        {
            Enter();
            active = true;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        active = false;
        Exit();
    }



    protected void ExitState()
    {
        active = false;
        Exit();
    }

    protected virtual void Initialize() { }
    protected virtual void Enter() { }
    protected virtual void Exit() { }
    protected virtual void Update() { }
}
