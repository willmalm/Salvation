using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBlackboard : MonoBehaviour
{
    public PatrolRoute patrolRoute;

    public bool loopPatrol;

    public GameObject currentTarget;

    private new Rigidbody rigidbody;
    private Animator animator;
    private NavMeshAgent navigation;

    // Use this for initialization
    void Start ()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        navigation = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        EventManager.Subscribe("PauseGame", Disable);
        EventManager.Subscribe("ContinueGame", Enable);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe("PauseGame", Disable);
        EventManager.Unsubscribe("ContinueGame", Enable);
    }

    private void Disable(object argument)
    {
        animator.speed = 0;
        rigidbody.isKinematic = true;
        navigation.enabled = false;
    }

    private void Enable(object argument)
    {
        animator.speed = 1;
        rigidbody.isKinematic = false;
        navigation.enabled = true;
    }
}
