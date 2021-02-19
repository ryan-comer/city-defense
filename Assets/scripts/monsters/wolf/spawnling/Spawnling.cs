using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnling : MonoBehaviour
{

    public float attackRange;   // Range to do a normal attack
    public float leapRange; // Range to do a leaping attack

    public float attackCooldown;    // Normal attack cooldown
    public float leapCooldown;  // Leap ability cooldown

    private Cooldowns cooldowns;    // Manages the cooldowns for abilities
    private MonsterTargeting monsterTargeting;  // Controls the target of the monster
    private Animator anim;  // Animator for the spawnling

    private const string LEAP_NAME = "leap";
    private const string ATTACK_NAME = "attack";

    private void Start()
    {
        cooldowns = GetComponent<Cooldowns>();
        monsterTargeting = GetComponent<MonsterTargeting>();
        anim = GetComponent<Animator>();

        Debug.Assert(cooldowns);
        Debug.Assert(monsterTargeting);
        Debug.Assert(anim);

        registerCooldowns();
    }

    private void Update()
    {
        useAbilities();
    }

    // See if you can use abilities
    private void useAbilities()
    {
        if (cooldowns.GetTimeLeft(LEAP_NAME) <= 0 && checkAbilityRange(leapRange))
        {
            // Leap attack

            cooldowns.StartCooldown(LEAP_NAME);
        }else if(cooldowns.GetTimeLeft(ATTACK_NAME) <= 0 && checkAbilityRange(attackRange))
        {
            // Attack
            anim.SetTrigger("attack");
            cooldowns.StartCooldown(ATTACK_NAME);
        }
    }

    // Register all the cooldowns for the spawnling
    private void registerCooldowns()
    {
        cooldowns.RegisterCooldown(ATTACK_NAME, attackCooldown);
        cooldowns.RegisterCooldown(LEAP_NAME, leapCooldown);
    }

    // Check if an ability is in range of the target
    private bool checkAbilityRange(float range)
    {
        GameObject target = monsterTargeting.FindTarget();
        if(target == null)
        {
            // No target
            return false;
        }

        // See if you're close enough
        if ((target.transform.position - transform.position).magnitude < range)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
