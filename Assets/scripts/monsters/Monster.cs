using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterThreat
{
    WOLF,
    TIGER,
    DEMON,
    DRAGON,
    GOD
}

// Represents a monster that the player fights
public class Monster : MonoBehaviour
{

    public MonsterThreat threat;    // The threat level of this monster

    private MonsterTargeting monsterTargeting; // Used to determine what the monster targets
    private MonsterMovement monsterMovement;    // Controls movement related logic for the monster
    private Combat combat;  // Controls combat related logic for the monster

    private GameObject currentMonsterTarget;    // The object that this monster is targeting

    private Vector3 moveVector; // Vector the monster should move this frame

    private void Start()
    {
        monsterTargeting = GetComponent<MonsterTargeting>();
        monsterMovement = GetComponent<MonsterMovement>();
        combat = GetComponent<Combat>();

        Debug.Assert(monsterTargeting);
        Debug.Assert(monsterMovement);
        Debug.Assert(combat);
    }

    private void Update()
    {
        findTarget();
    }

    private void FixedUpdate()
    {
        moveMonster();
    }

    // Find a target for the monster
    private void findTarget()
    {
        currentMonsterTarget = monsterTargeting.FindTarget();
    }

    // Move the monster towards the target
    private void moveMonster()
    {
        monsterMovement.MoveMonster(currentMonsterTarget);
    }

}
