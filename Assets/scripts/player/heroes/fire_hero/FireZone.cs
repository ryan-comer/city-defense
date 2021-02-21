using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireZone : MonoBehaviour
{
    public float tickRate;  // How many times per second to tick damage
    public float damagePerTick = 4;  // How much damage per tick to do
    public float duration;  // How long the firezone lasts

    HashSet<Combat> charactersHit = new HashSet<Combat>();    // Keep track of who will take damage

    private void Start()
    {
        StartCoroutine(damageCo());
        Destroy(gameObject, duration);
    }

    // Damage characters who are in the collider
    private IEnumerator damageCo()
    {
        while (true)
        {
            foreach (Combat combat in charactersHit)
            {
                if (combat)
                {
                    combat.Damage(damagePerTick);
                }
            }

            yield return new WaitForSeconds(tickRate);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "monster")
        {
            return;
        }

        // Check for combat
        Combat combat = other.gameObject.GetComponent<Combat>();
        if (combat)
        {
            charactersHit.Add(combat);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "monster")
        {
            return;
        }

        // Check for combat
        Combat combat = other.gameObject.GetComponent<Combat>();
        if (combat)
        {
            charactersHit.Remove(combat);
        }
    }

}
