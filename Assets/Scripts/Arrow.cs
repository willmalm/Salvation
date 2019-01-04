using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody rigidbody;
    Material material;

    public string enemyTag = "Enemy";
    public int damageDealt = 20;

    private float spawnTime;
    private float lifeTime = 20f;
    private bool hitObject;

	void Start ()
    {
        spawnTime = Time.time;
        rigidbody = GetComponent<Rigidbody>();
        material = GetComponent<Renderer>().material;
	}
	
	void Update ()
    {
        RaycastHit hit;

        if (Time.time - spawnTime > lifeTime)
            Destroy(gameObject);
        else if (Time.time - spawnTime > lifeTime - 2f)
        {
            Debug.Log("yo");
            material.SetFloat("_Dissolve", 1 - (lifeTime - (Time.time - spawnTime))/2);
        }

        if (Physics.Raycast(transform.position - transform.forward * 3, transform.forward, out hit, 4.5f) && !hitObject)
        {
            if (hit.collider.CompareTag(enemyTag))
            {
                DamageInstance damageInstance = new DamageInstance();
                damageInstance.damage = damageDealt;
                EventManager.TriggerEvent("damage", damageInstance, hit.collider.gameObject);
                Destroy(gameObject);
            }

            if (hit.transform.gameObject.isStatic)
            {
                hitObject = true;
                //rigidbody.position = rigidbody.position - transform.forward * hit.distance;
                transform.position = hit.point - transform.forward * 1.5f;
                rigidbody.velocity = Vector3.zero;
                rigidbody.useGravity = false;
                //Destroy(gameObject);
            }
        }

        if (!hitObject)
        {
            Quaternion newDirection = Quaternion.LookRotation(rigidbody.velocity);

            if (rigidbody.velocity.magnitude > 0.4f)
                transform.rotation = newDirection;
        }
	}
}
