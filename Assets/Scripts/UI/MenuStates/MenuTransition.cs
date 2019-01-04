using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MenuTransition : FSMState
{
    GameObject fromObject;
    FSMState toState;
    Animator animator;

    public MenuTransition(FSMState toState, Animator animator)
    {
        //this.fromObject = fromObject;
        this.toState = toState;
        this.animator = animator;
    }

    public override void Enter()
    {
        animator.SetTrigger("changeMenu");
    }

    public override void Execute()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("MenuStart"))
        {
            //fromObject.SetActive(false);

            UIManager.Instance.fsm.ChangeState(toState);
        }
    }

    public override void Exit()
    {
        
    }
}
