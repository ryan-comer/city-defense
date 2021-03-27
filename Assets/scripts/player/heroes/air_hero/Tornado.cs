using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{

    public float strength;  // The strength of the pull
    public float duration;  // How long the tornado lasts
    public LayerMask affectedLayers;    // The layers to affect with the pull

    private HashSet<Rigidbody> affectedObjects = new HashSet<Rigidbody>();  // Set of objects that are being pulled towards the center

    private void Start()
    {
        Destroy(gameObject, duration);
    }

    private void FixedUpdate()
    {
        foreach(Rigidbody rigidBody in affectedObjects)
        {
            if(rigidBody == null)
            {
                continue;
            }

            Vector3 pullDirection = transform.position - rigidBody.transform.position;
            pullDirection.y = 0;
            pullDirection.Normalize();

            rigidBody.AddForce(pullDirection * strength);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check layer
        if(Utils.IsLayerInMask(other.gameObject.layer, affectedLayers))
        {
            // Check rigidbody
            Rigidbody rigidBody = other.GetComponent<Rigidbody>();
            if(rigidBody != null)
            {
                affectedObjects.Add(rigidBody);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check layer
        if(Utils.IsLayerInMask(other.gameObject.layer, affectedLayers))
        {
            // Check rigidbody
            Rigidbody rigidBody = other.GetComponent<Rigidbody>();
            if(rigidBody != null)
            {
                if (affectedObjects.Contains(rigidBody))
                {
                    affectedObjects.Remove(rigidBody);
                }
            }
        }
    }
}
