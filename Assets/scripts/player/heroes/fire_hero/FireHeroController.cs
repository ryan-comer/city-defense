using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHeroController : MonoBehaviour
{
    public GameObject leftHand;
    public GameObject rightHand;

    public FireBall fireball_p;
    public FlameThrower flamethrower_p;

    private PlayerMovement playerMovement;
    private Animator anim;

    private FlameThrower currentFlameThrower;   // Current flamethrower object, used to destroy when done

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

    private void Start()
    {
        anim = GetComponent<Animator>();
        Debug.Assert(anim);

        playerMovement = GetComponent<PlayerMovement>();
        Debug.Assert(playerMovement);

        Debug.Assert(leftHand);
        Debug.Assert(rightHand);
    }

    private void Update()
    {
        checkInput();
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

}
