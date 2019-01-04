using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : Interactable
{
    void OnEnable()
    {
        EventManager.Subscribe("Interact", Interact, gameObject);
    }

    void OnDisable()
    {
        EventManager.Unsubscribe("Interact", Interact);
    }

    void Interact(object argument)
    {
        bool interact = (bool)argument;

        if (interact)
        {
            Debug.Log("interacted");
            DialogueManager.Instance.SayLine("English", "ULR_GREET01", "me");
        }
        else
        {

        }
    }

}
