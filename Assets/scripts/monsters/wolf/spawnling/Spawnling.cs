using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnling : MonoBehaviour
{

    public float attackRange;   // Range to do a normal attack
    public float leapRange; // Range to do a leaping attack

    public float attackCooldown;    // Normal attack cooldown
    public float leapCooldown;  // Leap ability cooldown

    public float attackDamage;  // Damage of attack
    public float leapDamage;    // Damage of leap

    public BoxCollider meleeCollider;  // Collider used for melee attack detection

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

    // Called from animation
    public void Attack()
    {
        Collider[] hits = getMeleeHits();

        // Damage all the combats
        foreach(Collider hitCollider in hits)
        {
            Combat combat = hitCollider.gameObject.GetComponent<Combat>();
            if (combat)
            {
                combat.Damage(attackDamage);
            }
        }
    }

    // Called from animation
    public void Leap()
    {
        Collider[] hits = getMeleeHits();

        // Damage all the combats
        foreach(Collider hitCollider in hits)
        {
            Combat combat = hitCollider.gameObject.GetComponent<Combat>();
            if (combat)
            {
                combat.Damage(leapDamage);
            }
        }
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

    private Collider[] getMeleeHits()
    {
        int layerMask = LayerMask.GetMask("player", "citizen", "building");
        Collider[] colliders = Physics.OverlapBox(meleeCollider.transform.position + meleeCollider.center, new Vector3(
            meleeCollider.size.x/2,
            meleeCollider.size.y/2,
            meleeCollider.size.z/2
        ), Quaternion.LookRotation(transform.forward, Vector3.up), layerMask);

        return colliders;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(meleeCollider.transform.position + meleeCollider.center, new Vector3(
            meleeCollider.size.x / 2,
            meleeCollider.size.y / 2,
            meleeCollider.size.z / 2
        ));
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
