using UnityEngine;

public class MenuVideo : MenuFSMState
{
    public MenuVideo(GameObject menuObject)
    {
        this.menuObject = menuObject;
    }

    public override void Enter()
    {
       // menuObject.SetActive(true);
    }
}
