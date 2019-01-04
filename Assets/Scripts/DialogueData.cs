using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueData : MonoBehaviour 
{
    public float minDistance;

    private GameObject player;


	void Start () 
	{
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	void Update () 
	{
		if (Vector3.Distance(transform.position, player.transform.position) <= minDistance)
        {
            EventManager.TriggerEvent("DialogueRequest", gameObject, player);
        }
	}
}
