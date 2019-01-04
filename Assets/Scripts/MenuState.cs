using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFSMState : FSMState
{
    protected GameObject menuObject;

    public override void Execute()
    {
        if (!menuObject.activeSelf && UIManager.Instance.menuAnimator.GetCurrentAnimatorStateInfo(0).IsName("MenuStart"))
        {
            menuObject.SetActive(true);
        }
    }

    public override void Exit()
    {
        //menuObject.SetActive(false);
        UIManager.Instance.menuAnimator.SetTrigger("changeMenu");
    }
}
