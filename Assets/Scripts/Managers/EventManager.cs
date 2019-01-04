using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EventManager
{
    class Event
    {
        public Action<object> method;
        public object targetObject;
        public string name;

        public Event(string name, Action<object> method, object targetObject = null)
        {
            this.name = name;
            this.method = method;
            this.targetObject = targetObject;
        }
    }

    List<Event> subscriptions = new List<Event>();

    static EventManager instance;

    static public EventManager Instance
    {
        get
        {
            if (instance == null)
                instance = new EventManager();
            return instance;
        }
        set
        {
            instance = value;
        }
    }

    public static void Subscribe(string eventName, Action<object> method, object targetObject = null)
    {
        Instance.subscriptions.Add(new Event(eventName, method, targetObject));
    }

    public static void Unsubscribe(string eventName, Action<object> method)
    {
        var item = Instance.subscriptions.First(element => element.name == eventName && element.method == method);
        Instance.subscriptions.Remove(item);
    }

    public static void TriggerEvent(string eventName, object argument, object targetObject = null)
    {
        int receivers = 0;

        foreach (var item in Instance.subscriptions.Where(element => element.name == eventName && (element.targetObject == targetObject || element.targetObject == null)).ToList())
        {
            item.method(argument);
            receivers++;
        }

        Debug.Log("Triggered event: " + eventName + ", " + receivers + " receiver(s)");
    }
}
