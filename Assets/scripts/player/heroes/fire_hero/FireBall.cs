using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{

    public float moveSpeed = 1.0f;  // Speed that the fireball moves

    private Vector3 target;
    private Vector3 origin;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 20.0f); // Destroy after 20 seconds
    }

    // Update is called once per frame
    void Update()
    {
        moveForward();
    }

    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    public void SetTarget(Vector3 target)
    {
        this.target = target;
    }

    private void moveForward()
    {
        Vector3 moveDirection = target - origin;
        moveDirection.Normalize();
        moveDirection *= Time.deltaTime * moveSpeed;

        transform.Translate(moveDirection, Space.World);
    }

}
