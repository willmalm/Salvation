using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public DamageTrigger damageTrigger;

    public void EnableTrigger()
    {
        damageTrigger.Enable();
    }

    public void DisableTrigger()
    {
        damageTrigger.Disable();
    }
}
