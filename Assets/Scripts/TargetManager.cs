using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager
{
    public static TargetManager _instance;

    public static TargetManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new TargetManager();

            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

    List<Target> targets = new List<Target>();


    public static void AddCharacter(Target target)
    {
        if (target)
            Instance.targets.Add(target);
    }

    public static List<Target> GetCharacters()
    {
        return Instance.targets;
    }
}
