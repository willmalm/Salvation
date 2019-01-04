using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MenuLoadGame : MenuFSMState
{
    public MenuLoadGame(GameObject menuObject)
    {
        this.menuObject = menuObject;
    }

    public override void Enter()
    {
        for (int i = 0; i < 3; i++)
        {
            if (File.Exists("Assets/Resources/Save_" + i))
                menuObject.transform.GetChild(i).gameObject.SetActive(true);
            else
                menuObject.transform.GetChild(i).gameObject.SetActive(false);
        }

        Button firstButton = menuObject.transform.GetChild(0).GetComponent<Button>();
        firstButton.Select();
        firstButton.OnSelect(null);
    }
}
