using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VolumetricLines;

[RequireComponent(typeof(VolumetricLineBehavior))]
public class LaserBeam : MonoBehaviour
{
    public Vector3 StartPosition
    {
        get
        {
            return transform.TransformPoint(line.StartPos);
        }
        set
        {
            line.StartPos = transform.InverseTransformPoint(value);
        }
    }
    public Vector3 EndPosition
    {
        get
        {
            return transform.TransformPoint(line.EndPos);
        }
        set
        {
            line.EndPos = transform.InverseTransformPoint(value);
        }
    }
    public Vector3 TargetPosition
    {
        get
        {
            return targetPosition;
        }
        set
        {
            if(targetPosition.magnitude == Vector3.positiveInfinity.magnitude)
            {
                EndPosition = value;
            }

            targetPosition = value;
        }
    }

    public float damagePerTick;
    public float tickRate;
    public float moveSpeed; // How fast does the laser move

    public VolumetricLineBehavior line;

    private Vector3 targetPosition = Vector3.positiveInfinity;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(damageCo());
    }

    void Update()
    {
        moveLaserTowardsTarget();
    }

    // Move the laser towards the target at a certain rate
    private void moveLaserTowardsTarget()
    {
        Vector3 moveDirection = targetPosition - EndPosition;
        moveDirection.Normalize();

        EndPosition = EndPosition + (moveDirection * Time.deltaTime * moveSpeed);
    }

    private IEnumerator damageCo()
    {
        while (true)
        {
            damage();
            yield return new WaitForSeconds(tickRate);
        }
    }

    private void damage()
    {
        // Check for hits
        float beamLength = (EndPosition - StartPosition).magnitude;
        float beamWidth = line.LineWidth / 3;
        Vector3 center = Vector3.Lerp(StartPosition, EndPosition, 0.5f);
        Quaternion orientation = Quaternion.LookRotation((StartPosition - EndPosition));

        Collider[] hits = Physics.OverlapBox(center, new Vector3(
            beamWidth/2,
            beamWidth/2,
            beamLength/2
        ), orientation, LayerMask.GetMask("player_main", "building"));

        foreach(Collider hit in hits)
        {
            // Check for combat
            Combat combat = hit.GetComponent<Combat>();
            if (combat)
            {
                combat.Damage(damagePerTick);
            }
        }
    }
}
