using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatSeekerBall : MonoBehaviour
{

    public float damage;    // Damage per ball
    public float moveSpeed; // How fast the balls move
    public float delayAtInitialPosition = 1.0f;    // How long to stay at the initial position

    private bool shouldMoveToInitialPosition = true;    // Used to prevent jumping around at the initial position
    private bool reachedInitialPosition = false;    // Has the ball reached the initial position yet
    private GameObject target;  // Target that the ball is tracking

    private Vector3 initialPosition;    // Initial position for the ball to hit
    public Vector3 InitialPosition
    {
        get
        {
            return initialPosition;
        }
        set
        {
            initialPosition = value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        checkReachedInitialPosition();
        if (!reachedInitialPosition)
        {
            if (shouldMoveToInitialPosition)
            {
                moveToInitialPosition();
            }
        }
        else
        {
            moveToTarget();
        }
    }

    private void checkReachedInitialPosition()
    {
        if((initialPosition - transform.position).magnitude < 0.1f)
        {
            findTarget();
            shouldMoveToInitialPosition = false;
            StartCoroutine(delaySetReachInitialPositionCo());
        }
    }

    private IEnumerator delaySetReachInitialPositionCo()
    {
        yield return new WaitForSeconds(delayAtInitialPosition);
        reachedInitialPosition = true;
    }

    private void moveToInitialPosition()
    {
        Vector3 moveVector = initialPosition - transform.position;
        moveVector.Normalize();
        moveVector *= Time.deltaTime * moveSpeed;

        transform.Translate(moveVector);
    }

    private void moveToTarget()
    {
        // No target in range
        if(target == null)
        {
            findTarget();   // Find a new target

            if(target == null)
            {
                Destroy(gameObject);    // No more targets
                return;
            }
        }

        Vector3 moveVector = target.transform.position - transform.position;
        moveVector.Normalize();
        moveVector *= Time.deltaTime * moveSpeed;

        transform.Translate(moveVector);
    }

    // Find the closest monster to attack
    private void findTarget()
    {
        int layerMask = LayerMask.GetMask("monster");
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2000, layerMask);

        // No colliders found
        if(colliders.Length == 0)
        {
            target = null;
            return;
        }

        List<Collider> collidersList = new List<Collider>(colliders);
        collidersList.Sort((a, b) =>
        {
            float distanceA = (a.transform.position - transform.position).magnitude;
            float distanceB = (b.transform.position - transform.position).magnitude;

            return distanceA.CompareTo(distanceB);
        });

        // Randomly pick the closest 10
        int maxIndex = Mathf.Min(10, collidersList.Count);
        target = collidersList[Random.Range(0, maxIndex)].gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != target)
        {
            return;
        }

        // Hit target
        Combat combat = other.gameObject.GetComponent<Combat>();
        if (combat)
        {
            combat.Damage(damage);
            Destroy(gameObject);
        }
    }

}
