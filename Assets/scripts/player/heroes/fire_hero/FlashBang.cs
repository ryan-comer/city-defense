using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashBang : MonoBehaviour
{

    public float damage;    // How much damage to do
    public float stunDuration;  // How long to stun

    private BoxCollider flashBangCollider;   // Collider to use for aoe

    // Start is called before the first frame update
    void Start()
    {
        flashBangCollider = GetComponent<BoxCollider>();
        Debug.Assert(flashBangCollider);

        flashBang();
    }

    // Do the flashbang effects
    private void flashBang()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + flashBangCollider.center, new Vector3(
            flashBangCollider.size.x / 2,
            flashBangCollider.size.y / 2,
            flashBangCollider.size.z / 2
        ));

        foreach (Collider collider in colliders)
        {
            // Only hit monsters
            if(collider.gameObject.tag != "monster")
            {
                continue;
            }

            // Check for combat
            Combat combat = collider.GetComponent<Combat>();
            if (combat)
            {
                combat.Damage(damage);
            }

            // Check for status
            StatusEffects statusEffects = collider.GetComponent<StatusEffects>();
            if (statusEffects)
            {
                statusEffects.AddStatus(STATUS_EFFECT.STUN, stunDuration);
            }
        }
    }

}
