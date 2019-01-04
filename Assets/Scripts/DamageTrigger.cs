using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    public int damageDealt;
    public bool active;
    public string enemyTag;
    private Collider weaponCollider;

    private List<GameObject> objectsHit = new List<GameObject>();

    private void Start()
    {
        weaponCollider = GetComponent<Collider>();
    }

    public void Disable()
    {
        weaponCollider.enabled = false;
        objectsHit.Clear();
    }

    public void Enable()
    {
        weaponCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(enemyTag) && !objectsHit.Contains(collider.gameObject))
        {
            DamageInstance damageInstance = new DamageInstance();
            damageInstance.damage = damageDealt;
            damageInstance.position = transform.position;
            EventManager.TriggerEvent("damage", damageInstance, collider.gameObject);
            objectsHit.Add(collider.gameObject);
            SavedData.Instance.spirit.Value += 10;
        }
    }
}
