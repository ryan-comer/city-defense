using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to find the target for the monster (civilians, buildings, heroes)
public class MonsterTargeting : MonoBehaviour
{

    public float sightRange = 5.0f;

    public GameObject FindTarget()
    {
        int layerMask = LayerMask.GetMask("player", "citizen");
        Collider[] collisions = Physics.OverlapSphere(transform.position, sightRange, layerMask);

        if(collisions.Length == 0)
        {
            return null;
        }

        return collisions[0].gameObject;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

}
