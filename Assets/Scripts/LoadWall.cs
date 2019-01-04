using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadWall : MonoBehaviour
{
    BoxCollider collider;

    public LoadWall neighbour;
    public int prefabIndex;

    public bool unload;

    public bool playerPassed = false;

	void Start ()
    {
        collider = GetComponent<BoxCollider>();
	}
	
	void Update ()
    {
		
	}

    private void OnDrawGizmos()
    {
        if (collider == null)
            collider = GetComponent<BoxCollider>();

        if (unload)
            Gizmos.color = new Color(1, 0, 0, 0.5f);
        else
            Gizmos.color = new Color(0, 0, 1, 0.5f);

        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

        Gizmos.DrawCube(Vector3.zero, collider.size);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Hit");
            playerPassed = true;

            if (neighbour.playerPassed)
            {
                if (unload)
                    WorldManager.UnloadWorldPrefab(neighbour.prefabIndex);
                else
                    WorldManager.LoadWorldPrefab(neighbour.prefabIndex);

                neighbour.playerPassed = false;
            }
        }
    }
}
