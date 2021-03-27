using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowards : MonoBehaviour
{

    public float moveSpeed; // How fast to move

    public Vector3 MoveDirection
    {
        get
        {
            return moveDirection;
        }
        set
        {
            moveDirection = value;
        }
    }
    private Vector3 moveDirection;  // Direction to move in

    // Update is called once per frame
    void Update()
    {
        move();
    }

    private void move()
    {
        transform.Translate(moveDirection.normalized * moveSpeed * Time.deltaTime, Space.World);
    }

}
