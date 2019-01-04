using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAbstract : MonoBehaviour
{
    bool hostile = true;



	void Start ()
    {
        EntityManager.AddCharacter(this);
	}
	
	void Update ()
    {
		
	}

    private void OnDestroy()
    {
        EntityManager.RemoveCharacter(this);
    }

        public bool IsHostile()
    {
        return hostile;
    }
}
