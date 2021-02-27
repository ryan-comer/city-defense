using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowards : MonoBehaviour
{

    public Vector3 MoveDirection;   // Direction to move in
    public float moveSpeed; // How fast to move

    // Update is called once per frame
    void Update()
    {
        move();
    }

    private void move()
    {
        transform.Translate(MoveDirection.normalized * moveSpeed * Time.deltaTime);
    }

}
