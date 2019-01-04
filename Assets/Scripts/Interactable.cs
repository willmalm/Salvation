using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    bool active = true;

    bool Active
    {
        get { return active; }
        set
        {
            active = value;
            EventManager.TriggerEvent("RemoveInteractable", gameObject);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (active && collider.tag == "Player")
        {
            EventManager.TriggerEvent("AddInteractable", gameObject);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            EventManager.TriggerEvent("RemoveInteractable", gameObject);
        }
    }
}
