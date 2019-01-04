using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    List<GameObject> interactables = new List<GameObject>();

	void Start ()
    {
		
	}
	
	void Update ()
    {
		if (Input.GetKeyDown("e") && interactables.Count > 0)
        {
            EventManager.TriggerEvent("Interact", true, interactables[0]);
        }
	}

    void OnEnable()
    {
        EventManager.Subscribe("AddInteractable", AddInteractable);
        EventManager.Subscribe("RemoveInteractable", RemoveInteractable);
    }

    void OnDisable()
    {
        EventManager.Unsubscribe("AddInteractable", AddInteractable);
        EventManager.Unsubscribe("RemoveInteractable", RemoveInteractable);
    }

    void AddInteractable(object argument)
    {
        GameObject interactable = (GameObject)argument;

        interactables.Add(interactable);
    }

    void RemoveInteractable(object argument)
    {
        GameObject interactable = (GameObject)argument;

        interactables.Remove(interactable);
    }

}
