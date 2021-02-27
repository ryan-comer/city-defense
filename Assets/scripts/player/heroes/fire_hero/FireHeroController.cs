using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHeroController : MonoBehaviour
{
    public GameObject leftHand;
    public GameObject rightHand;

    public FireBall fireball_p;
    public FlameThrower flamethrower_p;
    public ParticleSystem flightExhaust;
    public FireZone fireZone_p;
    public FlashBang flashbang_p;
    public HeatSeekerBall heatSeekerBall_p;

    public float heatSeekerBallFiringTime = 1.0f;   // How many seconds to fire off the balls
    public float heatSeekerBallFireTick = 0.2f; // Delay between firing balls
    public float heatSeekerBallInitialDistance = 20.0f; // How far does the ball shoot out before tracking
    public float flightSpeed;   // How fast the hero can fly

    private PlayerMovement playerMovement;
    private Animator anim;
    private Rigidbody rigid;

    private FlameThrower currentFlameThrower;   // Current flamethrower object, used to destroy when done

    private Vector3 flightMoveVector;   // Move vector used to fly
    private bool isFlying = false;  // Is the hero currently flying

    private Vector3 fireZonePosition;   // Where the fire zone will be placed

    // Start firing off balls
    public void HeatSeeker()
    {
        StartCoroutine(shootHeatSeekerBallCo());
    }

    // Coroutine to fire off balls
    private IEnumerator shootHeatSeekerBallCo()
    {
        System.DateTime startFiringTime = System.DateTime.Now;
        while ((System.DateTime.Now - startFiringTime).TotalSeconds < heatSeekerBallFiringTime)
        {
            // Create and launch a ball
            HeatSeekerBall heatSeekerBall = Instantiate(heatSeekerBall_p);
            heatSeekerBall.transform.position = transform.position;
            heatSeekerBall.InitialPosition = transform.position + (Random.insideUnitSphere * heatSeekerBallInitialDistance);

            yield return new WaitForSeconds(heatSeekerBallFireTick);
        }
    }

    // Called by animation to shoot a fireball
    public void ShootFireball(int hand)
    {
        FireBall fireball = null;
        switch (hand)
        {
            // Left hand
            case 0:
                fireball = Instantiate(fireball_p);
                fireball.SetOrigin(leftHand.transform.position);
                fireball.transform.position = leftHand.transform.position;
                break;

            // Right hand
            case 1:
                fireball = Instantiate(fireball_p);
                fireball.SetOrigin(rightHand.transform.position);
                fireball.transform.position = rightHand.transform.position;
                break;
        }

        fireball.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Vector3.up);

        // Find the fireball target
        var target = PlayerUtils.GetAbilityHitVector();
        fireball.SetTarget(target);
    }

    // Create the fire zone
    public void FireZone()
    {
        if(fireZonePosition != null)
        {
            FireZone fireZone = Instantiate(fireZone_p);
            fireZone.transform.position = fireZonePosition;

            anim.ResetTrigger("firezone");
        }
    }

    // Create a flashbang and damage/stun enemies
    public void FlashBang()
    {
        // Create the particle effect
        FlashBang flashBang = Instantiate(flashbang_p);
        flashBang.transform.position = transform.position;

        // Destroy the particle effect
        Destroy(flashBang.gameObject, 1.0f);
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        rigid = GetComponent<Rigidbody>();

        Debug.Assert(playerMovement);
        Debug.Assert(anim);
        Debug.Assert(rigid);

        Debug.Assert(leftHand);
        Debug.Assert(rightHand);

        flightExhaust.Stop();   // Start stopped
    }

    private void Update()
    {
        checkInput();

        if (isFlying)
        {
            flight();
        }
    }

    private void FixedUpdate()
    {
        if (isFlying)
        {
            flightMovePlayer();
        }
    }

    private void checkInput()
    {
        if (Input.GetMouseButton(0))
        {
            fireBall();
        }
        if (Input.GetMouseButtonDown(1))
        {
            flameThrower(true);
        }
        if (Input.GetMouseButtonUp(1))
        {
            flameThrower(false);
        }
        if (Input.GetMouseButton(1))
        {
            playerMovement.ShouldFaceForward = true;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            toggleFlying();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            flashBang();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            fireZone();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            heatSeeker();
        }
    }

    // Play the heat seeker animation
    private void heatSeeker()
    {
        anim.SetTrigger("heatseeker");
    }

    // Start the animation for flashbang
    private void flashBang()
    {
        anim.SetTrigger("flashbang");
    }

    // Start animation to create a fire zone
    private void fireZone()
    {
        // Check for ground target
        if (PlayerUtils.GetTarget(out fireZonePosition, LayerMask.GetMask("monster_main", "ground")))
        {
            anim.SetTrigger("firezone");
        }
    }

    private void toggleFlying()
    {
        // Cancel flight
        if (isFlying)
        {
            rigid.useGravity = true;
            playerMovement.ShouldMove = true;
            StartCoroutine(delaySetShouldFaceCo());
            isFlying = false;

            resetRotation();

            anim.SetBool("flying", false);
            flightExhaust.Stop();
            return;
        }
        // Start flying
        else
        {
            rigid.useGravity = false;
            playerMovement.ShouldMove = false;
            playerMovement.ShouldFacePlayer = false;
            isFlying = true;

            anim.SetBool("flying", true);
            flightExhaust.Play();
            return;
        }
    }

    // Fireball ability
    private void fireBall()
    {
        anim.SetTrigger("fireball");
        playerMovement.ShouldFaceForward = true;
    }

    // Flamethrower ability
    private void flameThrower(bool value)
    {
        anim.SetBool("flamethrower", value);

        if (value)
        {
            // Create the flamethrower object
            currentFlameThrower = Instantiate(flamethrower_p, transform);
            currentFlameThrower.transform.position = Vector3.Lerp(leftHand.transform.position, rightHand.transform.position, 0.5f);
            currentFlameThrower.transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
        }
        else
        {
            Destroy(currentFlameThrower.gameObject);
        }
    }

    // Move the character according to flight
    private void flight()
    {
        setFlightMovement();
        flightFacePlayer();
    }

    // Set the move vector for flying
    private void setFlightMovement()
    {
        flightMoveVector = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            flightMoveVector += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            flightMoveVector += Vector3.left;
        }
        if (Input.GetKey(KeyCode.S))
        {
            flightMoveVector += Vector3.back;
        }
        if (Input.GetKey(KeyCode.D))
        {
            flightMoveVector += Vector3.right;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            flightMoveVector += Vector3.up;
        }
        if(Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.LeftControl))
        {
            flightMoveVector += Vector3.down;
        }

        flightMoveVector.Normalize();
        flightMoveVector *= flightSpeed;
        flightMoveVector = Camera.main.transform.TransformDirection(flightMoveVector);
    }

    // Face the player - flying
    private void flightFacePlayer()
    {
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
    }

    // Move the player - flying
    private void flightMovePlayer()
    {
        rigid.velocity = Vector3.zero;
        rigid.MovePosition(transform.position + flightMoveVector);
    }

    // Reset the rotation after flying
    private void resetRotation()
    {
        Vector3 direction = Camera.main.transform.forward;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private IEnumerator delaySetShouldFaceCo()
    {
        yield return new WaitForSeconds(1.0f);
        playerMovement.ShouldFacePlayer = true;
    }

}
