using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{

    public float moveSpeed = 5.0f;  // The move speed of the monster

    private Animator anim;
    private Rigidbody rigid;

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

        Vector3 moveVector = target.transform.position - transform.position;
        moveVector.Normalize();
        moveVector *= moveSpeed * Time.deltaTime;
        moveVector.y = 0;

        rigid.MovePosition(transform.position + moveVector);

        transform.rotation = Quaternion.LookRotation(new Vector3(
            moveVector.x,
            0,
            moveVector.z
        ), Vector3.up);

        anim.SetBool("walking", true);
    }

}
