using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerUtils
{

    // Find a point in a straight line to launch ability
    public static Vector3 GetAbilityHitVector()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        int layerMask = LayerMask.GetMask("ground", "monster");
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 200, layerMask))
        {
            return hit.point;
        }
        else
        {
            return ray.origin + (ray.direction * 200);
        }
    }

    // Get a ground target vector based on camera
    public static bool GetGroundTarget(out Vector3 groundTarget)
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        int layerMask = LayerMask.GetMask("ground");
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 1000.0f, layerMask))
        {
            groundTarget = hit.point;
            return true;
        }
        else
        {
            groundTarget = Vector3.zero;
            return false;
        }
    }
    
}
