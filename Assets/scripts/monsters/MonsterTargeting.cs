using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to find the target for the monster (civilians, buildings, heroes)
public class MonsterTargeting : MonoBehaviour
{
    public GameObject CurrentTarget
    {
        get
        {
            return currentTarget;
        }
    }
    public Vector3 CurrentTargetLocation
    {
        get
        {
            return currentTargetLocation;
        }
    }

    public float sightRange = 5.0f;

    private GameObject currentTarget;   // The current monster target
    private Vector3 currentTargetLocation;  // The location of the hit for the current target

    public GameObject FindTarget()
    {
        int layerMask = LayerMask.GetMask("player_main", "citizen", "building");
        Collider[] colliders = Physics.OverlapSphere(transform.position, sightRange, layerMask);

        if(colliders.Length == 0)
        {
            currentTarget = null;
            currentTargetLocation = Vector3.zero;
            return currentTarget;
        }

        // Find the closest collider
        List<Collider> collidersList = new List<Collider>(colliders);
        collidersList.Sort((a, b) =>
        {
            float distanceA = (a.ClosestPoint(transform.position) - transform.position).magnitude;
            float distanceB = (b.ClosestPoint(transform.position) - transform.position).magnitude;

            return distanceA.CompareTo(distanceB);
        });

        currentTargetLocation = collidersList[0].ClosestPoint(transform.position);
        currentTarget = collidersList[0].gameObject;

        return currentTarget;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

}
