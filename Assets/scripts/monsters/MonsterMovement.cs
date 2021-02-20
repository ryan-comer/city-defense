using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{

    public float moveSpeed = 5.0f;  // The move speed of the monster
    public float stoppingDistance;   // Don't get any closer to target

    public float groundCheckOffset; // How much to offset the ground check
    public float groundCheckDistance;   // How far to check for ground

    private Animator anim;
    private Rigidbody rigid;

    private bool m_shouldMove = true;
    public bool ShouldMove
    {
        get
        {
            return m_shouldMove;
        }
        set
        {
            m_shouldMove = value;
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
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();

        Debug.Assert(anim);
        Debug.Assert(rigid);
    }

    // Move the monster towards the target
    public void MoveMonster(GameObject target)
    {
        if(target == null)
        {
            anim.SetBool("walking", false);
            return;
        }

        if((target.transform.position - transform.position).magnitude <= stoppingDistance)
        {
            // Don't get closer
            return;
        }

        Vector3 moveVector = target.transform.position - transform.position;
        moveVector.Normalize();
        moveVector *= moveSpeed * Time.deltaTime;
        moveVector.y = 0;

        // Check if you should move
        if (m_shouldMove)
        {
            rigid.MovePosition(transform.position + moveVector);
        }

        transform.rotation = Quaternion.LookRotation(new Vector3(
            moveVector.x,
            0,
            moveVector.z
        ), Vector3.up);

        anim.SetBool("walking", true);
    }

    private bool isGrounded()
    {
        int layerMask = LayerMask.GetMask("ground", "building");
        Ray ray = new Ray(
            transform.position + Vector3.up*groundCheckOffset,
            Vector3.down
        );
        return Physics.Raycast(ray, groundCheckDistance, layerMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(
            transform.position + Vector3.up*groundCheckOffset,
            transform.position + Vector3.up*groundCheckOffset + Vector3.down*groundCheckDistance
        );  
    }

}
