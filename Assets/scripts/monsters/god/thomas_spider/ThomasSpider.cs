using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThomasSpider : MonoBehaviour
{
    public float slamCooldown;
    public float laserBallCooldown;
    public float laserBeamCooldown;

    public float slamRange;
    public float laserBallRange;
    public float laserBeamRange;

    public float slamDamage;

    public GameObject cannonStart;  // Where the cannon is located
    public GameObject laserBall_p;
    public LaserBeam laserBeam_p;
    public BoxCollider slamCollider;    // Collider used for slam ability

    private Cooldowns cooldowns;    // Manages the cooldowns for abilities
    private MonsterTargeting monsterTargeting;  // Controls the target of the monster
    private MonsterMovement monsterMovement;    // Controls the movement of the monster
    private Monster monster;    // Main monster component
    private Animator anim;  // Animator for the spawnling
    private Rigidbody rigid;    // Rigidbody for the spawnling
    private StatusEffects statusEffects;    // Status effects for the spawnling

    private const string SLAM_NAME = "slam";
    private const string LASER_BEAM_NAME = "laser_beam";
    private const string LASER_BALL_NAME = "laser_ball";

    private GameObject laserBeamPowerUp;    // Gameobject used to power up the laser beam
    private LaserBeam currentLaserBeam;    // Current instance of the laser beam

    private bool shouldUseAbilities = true;

    // Start is called before the first frame update
    void Start()
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

        GetComponent<Combat>().OnDeath += cleanup;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldUseAbilities)
        {
            useAbilities();
        }
    }

    // Slam in an area around the target
    public void Slam()
    {
        // Get the hits
        Collider[] hits = MonsterUtils.GetColliderHits(slamCollider, transform.localScale, LayerMask.GetMask("player_main", "building"));

        foreach(Collider collider in hits)
        {
            // Check for combat
            Combat combat = collider.GetComponent<Combat>();
            if (combat)
            {
                combat.Damage(slamDamage);
            }
        }
    }

    // Start firing a laser beam at target
    public void LaserBeam()
    {
        Destroy(laserBeamPowerUp);
        currentLaserBeam = Instantiate(laserBeam_p);

        StartCoroutine(laserBeamCo());
    }

    private void cleanup(GameObject obj)
    {
        if(currentLaserBeam?.gameObject != null)
        {
            Destroy(currentLaserBeam.gameObject);
        }
    }

    // Coroutine to manage the laser beam
    private IEnumerator laserBeamCo()
    {
        // First start position is almost hitting target
        currentLaserBeam.TargetPosition = Vector3.Lerp(cannonStart.transform.position, 
            monsterTargeting.CurrentTarget ? monsterTargeting.CurrentTarget.transform.position : cannonStart.transform.position, 
            0.8f);
        while(currentLaserBeam != null)
        {
            currentLaserBeam.StartPosition = cannonStart.transform.position;
            if(monsterTargeting.CurrentTarget == null)
            {
                currentLaserBeam.TargetPosition = cannonStart.transform.position;
            }
            else
            {
                currentLaserBeam.TargetPosition = monsterTargeting.CurrentTarget.transform.position;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public void LaserBeamDone()
    {
        Destroy(currentLaserBeam.gameObject);
        currentLaserBeam = null;

        monsterMovement.ShouldMove = true;
        shouldUseAbilities = true;
    }

    // Check cooldowns and use abilities
    private void useAbilities()
    {
        if (statusEffects.CheckStatus(STATUS_EFFECT.STUN))
        {
            // Can't use abilities - stunned
            return;
        }

        // Laser Beam
        if((cooldowns.GetTimeLeft(LASER_BEAM_NAME) <= 0) && MonsterUtils.CheckAbilityRange(transform, monsterTargeting, laserBeamRange))
        {
            laserBeam();
            cooldowns.StartCooldown(LASER_BEAM_NAME);
        }

        // Slam
        else if((cooldowns.GetTimeLeft(SLAM_NAME) <= 0) && MonsterUtils.CheckAbilityRange(transform, monsterTargeting, slamRange))
        {
            slam();
            cooldowns.StartCooldown(SLAM_NAME);
        }

        // Laser Ball
        else if((cooldowns.GetTimeLeft(LASER_BALL_NAME) <= 0) && MonsterUtils.CheckAbilityRange(transform, monsterTargeting, laserBallRange))
        {
            laserBall(monsterTargeting.CurrentTarget.transform.position);
            cooldowns.StartCooldown(LASER_BALL_NAME);
        }
    }

    private void laserBeam()
    {
        anim.SetTrigger("beam_power");
        laserBeamPowerUp = Instantiate(laserBall_p);
        Destroy(laserBeamPowerUp.GetComponent<MoveTowards>());
        laserBeamPowerUp.transform.position = cannonStart.transform.position;
        laserBeamPowerUp.transform.parent = cannonStart.transform;

        monsterMovement.ShouldMove = false;
        shouldUseAbilities = false;
    }

    private void slam()
    {
        anim.SetTrigger("slam");
    }

    // Fire a laser ball at the target
    private void laserBall(Vector3 targetPosition)
    {
        // Create the laser ball
        GameObject laserBall = Instantiate(laserBall_p);
        laserBall.transform.position = cannonStart.transform.position;

        // Set the ball direction
        Vector3 moveDirection = targetPosition - laserBall.transform.position;
        laserBall.GetComponent<MoveTowards>().MoveDirection = moveDirection;
        Destroy(laserBall.gameObject, 10.0f);
    }

    // Register all the cooldowns for Thomas
    private void registerCooldowns()
    {
        cooldowns.RegisterCooldown(SLAM_NAME, slamCooldown);
        cooldowns.RegisterCooldown(LASER_BEAM_NAME, laserBeamCooldown);
        cooldowns.RegisterCooldown(LASER_BALL_NAME, laserBallCooldown);
    }
}
