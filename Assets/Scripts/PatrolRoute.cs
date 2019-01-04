using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolRoute : MonoBehaviour
{
    public Transform[] positions;
    public Color color = Color.cyan;

	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    public bool IsValid()
    {
        if (positions == null || positions.Length < 2)
            return false;

        return true;
    }

    private void OnDrawGizmos()
    {
        if (!IsValid())
            return;

        Gizmos.color = color;

        for (int i = 0; i < positions.Length - 1; i++)
        {
            if (positions[i] != null && positions[i + 1] != null)
                Gizmos.DrawLine(positions[i].position, positions[i + 1].position);
        }
    }
}
