using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Senses : MonoBehaviour
{
    [Header("Vision")]
    public float viewDistance = 10;
    public float viewAngle = 90;
    public LayerMask blockageMask;
    public Transform eye;

    [Header("Hearing")]
    public float soundDistance;
    public float soundMultiplier;

    Transform lastTarget;

    void Start () {
		
	}
	
	void Update () {
		
	}

    public bool IsVisible(GameObject target)
    {
        
        lastTarget = null;
        if (!target)
            return false;

        
        float distance = Vector3.Distance(target.transform.position, eye.position);

        if (distance > viewDistance)
            return false;

        Vector3 viewDirection = (target.transform.position - eye.position).normalized;

        
        if (Vector3.Dot(viewDirection, transform.forward) < Mathf.Sin(viewAngle))
            return false;

        RaycastHit hit;

        if (Physics.Raycast(eye.position, viewDirection, out hit, distance, blockageMask))
            return false;

        lastTarget = target.transform;
        return true;
    }

    public bool IsNoisy(GameObject target)
    {
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        if (lastTarget)
        {
            Debug.Log(lastTarget.position);
            Gizmos.DrawSphere(lastTarget.position + Vector3.up * 4, 1f);
        }

        Gizmos.matrix = Matrix4x4.TRS(eye.position, transform.rotation, Vector3.one);

        Gizmos.DrawFrustum(Vector3.zero, viewAngle, viewDistance, 0, 1);        
    }
}
