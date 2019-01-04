using UnityEngine;

public class MenuMain : MenuFSMState
{
    public MenuMain(GameObject menuObject)
    {
        this.menuObject = menuObject;
    }

    public override void Enter()
    {
        int lastSave = GameManager.Settings.lastSave;

        if (lastSave < 0 || !GameManager.SaveExists(lastSave))
            menuObject.transform.GetChild(0).gameObject.SetActive(false);
        else
            menuObject.transform.GetChild(0).gameObject.SetActive(true);
    }  
}
