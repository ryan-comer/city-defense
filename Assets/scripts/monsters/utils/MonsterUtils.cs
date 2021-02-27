using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public static class MonsterUtils
{

    // Check if your ability is in range of a target
    public static bool CheckAbilityRange(Transform transform, MonsterTargeting monsterTargeting, float range)
    {
        GameObject target = monsterTargeting.CurrentTarget;
        Vector3 targetLocation = monsterTargeting.CurrentTargetLocation;

        if (target == null)
        {
            // No target
            return false;
        }

        // See if you're close enough
        if ((targetLocation - transform.position).magnitude < range)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Get all the colliders that hit by the passed collider
    public static Collider[] GetColliderHits(BoxCollider collider, Vector3 scale, int layerMask)
    {
        Vector3 halfExtents = collider.size;
        halfExtents.Scale(new Vector3(0.5f, 0.5f, 0.5f));
        halfExtents.Scale(scale);

        Vector3 center = collider.center;
        center = collider.transform.TransformPoint(center);

        Collider[] colliders = Physics.OverlapBox(center, halfExtents, Quaternion.LookRotation(collider.transform.forward, Vector3.up), layerMask);

        return colliders;
    }

}
