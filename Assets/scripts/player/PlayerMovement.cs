using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public GameObject playerBody;   // Used to face the player the right way
    public float moveSpeed;
    public float jumpHeight = 5.0f; // The height that the player can jump

    private Vector3 moveVector;
    private Rigidbody rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        Debug.Assert(rigidBody);
    }

    private void Update()
    {
        setMovement();
        checkJump();
        facePlayer();
    }

    // Move the player
    private void setMovement()
    {
        float moveX = 0.0f, moveZ = 0.0f;

        if (Input.GetKey(KeyCode.W))
        {
            moveZ += 1.0f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveX -= 1.0f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveZ -= 1.0f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveX += 1.0f;
        }

        moveVector = new Vector3
        {
            x = moveX,
            y = 0,
            z = moveZ
        };
        moveVector.Normalize();
        moveVector = Camera.main.transform.TransformDirection(moveVector);
        moveVector.y = 0;
        moveVector *= moveSpeed;
    }

    private void FixedUpdate()
    {
        movePlayer();
    }

    // Check if the player jumped
    private void checkJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigidBody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        }
    }

    // Move the player using physics
    private void movePlayer()
    {
        if(moveVector.magnitude > 0)
        {
            rigidBody.MovePosition(transform.position + moveVector); 
        }
    }

    // Face the player based on the camera
    private void facePlayer()
    {
        Vector3 direction = moveVector;
        direction.y = 0;
        if(direction.magnitude > 0)
        {
            Debug.Log(moveVector.magnitude);
            playerBody.transform.rotation = Quaternion.Lerp(playerBody.transform.rotation, Quaternion.LookRotation(direction, Vector3.up), 0.2f);
        }
    }

}
