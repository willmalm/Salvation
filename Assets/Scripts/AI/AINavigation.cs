using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AINavigation : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private new Rigidbody rigidbody;

    public float rotationSpeed;

	void Start ()
    {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.updateRotation = false;

        rigidbody = GetComponent<Rigidbody>();
	}
	
	void LateUpdate ()
    {
        Quaternion newRotation = transform.rotation;

        Vector3 walkDirection = navAgent.steeringTarget.ZeroY() - transform.position.ZeroY();

        if (walkDirection != Vector3.zero)
            newRotation = Quaternion.LookRotation(walkDirection);
        
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);

        //rigidbody.velocity = transform.forward * 
	}
}
