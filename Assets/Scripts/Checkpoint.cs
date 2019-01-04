using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointData
{
    public int level;
    public Vector3 position;
}

public class Checkpoint : MonoBehaviour
{

    public float interactRange;
    public bool playerInRange;

    public Transform player;

	void Start ()
    {
        //player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	void Update ()
    {
		if (Vector3.Distance(player.position, transform.position) < interactRange)
        {
            if (!playerInRange)
            {
                playerInRange = true;
                EventManager.TriggerEvent("AddInteractable", gameObject, player.gameObject);
            }            
        }
        else
        {
            if (playerInRange)
            {
                playerInRange = false;
                EventManager.TriggerEvent("RemoveInteractable", gameObject, player.gameObject);
            }
        }
    }

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        EventManager.Subscribe("Interact", Interact, gameObject);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe("Interact", Interact);
    }

    private void Interact(object argument)
    {
        CheckpointData newCheckpoint = new CheckpointData();
        newCheckpoint.level = 0;
        newCheckpoint.position = transform.position + Vector3.right;
        SavedData.Instance.lastCheckpoint = newCheckpoint;
    }
}
