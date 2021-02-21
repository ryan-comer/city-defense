using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnling : MonoBehaviour
{


    public float attackRange;   // Range to do a normal attack
    public float attackCooldown;    // Normal attack cooldown
    public float attackDamage;  // Damage of attack

    public float leapRange; // Range to do a leaping attack
    public float leapForceHorizontal; // How fast the leap happens
    public float leapForceVertical; // How fast the leap happens
    public float leapCooldown;  // Leap ability cooldown
    public float leapDamage;    // Damage of leap

    public BoxCollider meleeCollider;  // Collider used for melee attack detection

    private Cooldowns cooldowns;    // Manages the cooldowns for abilities
    private MonsterTargeting monsterTargeting;  // Controls the target of the monster
    private MonsterMovement monsterMovement;    // Controls the movement of the monster
    private Monster monster;    // Main monster component
    private Animator anim;  // Animator for the spawnling
    private Rigidbody rigid;    // Rigidbody for the spawnling
    private StatusEffects statusEffects;    // Status effects for the spawnling

    private bool abilitiesEnabled = true;    // Should use abilities

    private const string LEAP_NAME = "leap";
    private const string ATTACK_NAME = "attack";

    private void Start()
    {
        cooldowns = GetComponent<Cooldowns>();
        monster = GetComponent<Monster>();
        monsterTargeting = GetComponent<MonsterTargeting>();
        monsterMovement = GetComponent<MonsterMovement>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        statusEffects = GetComponent<StatusEffects>();

        Debug.Assert(cooldowns);
        Debug.Assert(monsterTargeting);
        Debug.Assert(monsterMovement);
        Debug.Assert(anim);
        Debug.Assert(rigid);
        Debug.Assert(statusEffects);

        registerCooldowns();
    }

    private void Update()
    {
        if (abilitiesEnabled)
        {
            useAbilities();
        }
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
        // Leap at your target
        monsterMovement.ShouldMove = false;
        StartCoroutine(leapCo());
    }

    // Coroutine to leap forward
    private IEnumerator leapCo()
    {
        // Check for target
        if(monsterTargeting.CurrentTarget == null)
        {
            anim.SetBool("leap", false);
            monsterMovement.ShouldMove = true;
            yield break;
        }

        // Apply the force to leap
        Vector3 targetPosition = monsterTargeting.CurrentTargetLocation;
        Vector3 moveVector = targetPosition - transform.position;
        moveVector.Normalize();
        moveVector *= leapForceHorizontal;
        moveVector.y = leapForceVertical;
        rigid.AddForce(moveVector);

        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();


        HashSet<Combat> combatsHit = new HashSet<Combat>(); // Used to make sure you only hit once
        while (true)
        {
            Collider[] hits = getMeleeHits();

            // Damage all the combats
            foreach(Collider hitCollider in hits)
            {
                Combat combat = hitCollider.gameObject.GetComponent<Combat>();
                if (combat && !combatsHit.Contains(combat))
                {
                    combat.Damage(leapDamage);
                    combatsHit.Add(combat);
                }
            }

            // Keep checking for hits or landed
            if(monsterMovement.IsGrounded)
            {
                anim.SetBool("leap", false);
                monsterMovement.ShouldMove = true;
                yield break;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    // See if you can use abilities
    private void useAbilities()
    {
        // Check for stun
        if (statusEffects.CheckStatus(STATUS_EFFECT.STUN))
        {
            return;
        }

        if (cooldowns.GetTimeLeft(LEAP_NAME) <= 0 && checkAbilityRange(leapRange))
        {
            // Leap attack
            anim.SetBool("leap", true);
            cooldowns.StartCooldown(LEAP_NAME);
        }
        else if(cooldowns.GetTimeLeft(ATTACK_NAME) <= 0 && checkAbilityRange(attackRange))
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
            meleeCollider.size.x,
            meleeCollider.size.y,
            meleeCollider.size.z
        ));
    }

    // Check if an ability is in range of the target
    private bool checkAbilityRange(float range)
    {
        GameObject target = monsterTargeting.CurrentTarget;
        Vector3 targetLocation = monsterTargeting.CurrentTargetLocation;

        if(target == null)
        {
            // No target
            return false;
        }

        // See if you're close enough
        if ((targetLocation - transform.position).magnitude < range)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
