using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirStrike : MonoBehaviour
{

    public void SetDirection(Vector3 newDirection)
    {
        MoveTowards moveTowards = GetComponent<MoveTowards>();
        moveTowards.MoveDirection = newDirection;
    }

}
