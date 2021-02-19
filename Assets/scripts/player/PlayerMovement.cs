using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public GameObject playerBody;   // Used to face the player the right way
    public float moveSpeed;
    public float jumpHeight = 5.0f; // The height that the player can jump

    public Vector3 groundCheckOffset;
    public float distanceToGround;

    private Vector3 moveVector;
    private Rigidbody rigidBody;

    private Animator anim;

    private bool shouldFaceForward = false;
    public bool ShouldFaceForward
    {
        set
        {
            shouldFaceForward = value;
            if (value)
            {
                lastFaceForwardTime = System.DateTime.Now;
            }
        }
    }

    private System.DateTime lastFaceForwardTime;
    private float maxFaceForwardTime = 2.0f;
    public float MaxFaceForwardTime
    {
        get
        {
            return maxFaceForwardTime;
        }
        set
        {
            maxFaceForwardTime = value;
        }
    }

    public bool IsGrounded
    {
        get
        {
            return isGrounded();
        }
    }

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        Debug.Assert(rigidBody);

        anim = GetComponent<Animator>();
        Debug.Assert(anim);
    }

    private void Update()
    {
        setMovement();
        checkJump();
        facePlayer();
        checkStopFacingForward();
    }

    private void checkStopFacingForward()
    {
        if((System.DateTime.Now - lastFaceForwardTime).TotalSeconds > maxFaceForwardTime)
        {
            ShouldFaceForward = false;
        }
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
        rigidBody.velocity = new Vector3(
            0.0f,
            rigidBody.velocity.y,
            0.0f
        );
        movePlayer();
    }

    // Check if the player jumped
    private void checkJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
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
            anim.SetBool("walking", true);
        }
        else
        {
            anim.SetBool("walking", false);
        }
    }

    // Face the player based on the camera
    private void facePlayer()
    {
        Vector3 direction = Vector3.zero;
        if (shouldFaceForward)
        {
            direction = Camera.main.transform.forward;
        }
        else
        {
            direction = moveVector;
        }
        direction.y = 0;
        if(direction.magnitude > 0)
        {
            transform.rotation = Quaternion.Lerp(playerBody.transform.rotation, Quaternion.LookRotation(direction, Vector3.up), 0.2f);
        }
    }

    // Determine if the gameobject is grounded
    private bool isGrounded()
    {
        Ray ray = new Ray(transform.position + groundCheckOffset, Vector3.down);
        return Physics.Raycast(ray, distanceToGround, LayerMask.GetMask(new string[] { "ground" }));
    }

    private void OnDrawGizmos()
    {
        Vector3 from = transform.position + groundCheckOffset;
        Vector3 to = new Vector3(from.x, from.y, from.z);
        to.y = to.y - distanceToGround;

        Gizmos.DrawLine(from, to);
    }

}
