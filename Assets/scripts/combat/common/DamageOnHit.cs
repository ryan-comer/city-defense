using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnHit : MonoBehaviour
{

    public LayerMask layerMask; // Layer mask of what you can hit
    public int damage;  // Damage caused when hit
    public bool destroyOnHit;   // Should this object be destroyed when it hits a combat

    public delegate void HitDelegate(Combat combatHit);
    public event HitDelegate OnHit;

    private void OnTriggerEnter(Collider other)
    {
        // Check layer
        if((layerMask.value & (1 << other.gameObject.layer)) == 0)
        {
            return;
        }

        // Check combat
        Combat combat = other.GetComponent<Combat>();
        if (combat)
        {
            combat.Damage(damage);
            if (destroyOnHit)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
