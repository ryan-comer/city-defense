using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : MonoBehaviour
{

    public float tickRate;  // How many times per second to tick damage
    public float damagePerTick = 4;  // How much damage per tick to do

    HashSet<Combat> charactersHit = new HashSet<Combat>();    // Keep track of who will take damage

    private void Start()
    {
        StartCoroutine(damageCo());
    }

    // Update is called once per frame
    void Update()
    {
        faceFlameThrower();
    }

    // Damage characters who are in the collider
    private IEnumerator damageCo()
    {
        while (true)
        {
            foreach(Combat combat in charactersHit)
            {
                if (combat)
                {
                    combat.Damage(damagePerTick);
                }
            }

            yield return new WaitForSeconds(tickRate);
        }
    }

    // Make the flamethrower face the right way
    private void faceFlameThrower()
    {
        transform.rotation = Quaternion.LookRotation(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)).direction, Vector3.up);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check for combat
        Combat combat = other.gameObject.GetComponent<Combat>();
        if (combat)
        {
            charactersHit.Add(combat);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check for combat
        Combat combat = other.gameObject.GetComponent<Combat>();
        if (combat)
        {
            charactersHit.Remove(combat);
        }
    }

}
