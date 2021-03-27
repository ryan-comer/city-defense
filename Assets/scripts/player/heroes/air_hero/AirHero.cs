using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirHero : MonoBehaviour
{
    public AirStrike airStrike_p;
    public Tornado tornado_p;

    public GameObject leftHand;
    public GameObject rightHand;

    private PlayerMovement playerMovement;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();

        Debug.Assert(playerMovement);
        Debug.Assert(anim);
    }

    // Update is called once per frame
    void Update()
    {
        checkInput();
    }

    // Cast an air strike
    public void Strike(int hand)
    {
        AirStrike newAirStrike = Instantiate(airStrike_p);
        Vector3 target = PlayerUtils.GetAbilityHitVector();
        switch (hand)
        {
            // Left
            case 0:
                newAirStrike.transform.position = leftHand.transform.position;
                break;
            // Right
            case 1:
                newAirStrike.transform.position = rightHand.transform.position;
                break;
        }

        Vector3 direction = target - newAirStrike.transform.position;
        newAirStrike.SetDirection(direction);
        newAirStrike.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

        Destroy(newAirStrike.gameObject, 10.0f);
    }

    public void Tornado()
    {
        // Get the target
        Vector3 target;
        if(PlayerUtils.GetTarget(out target, LayerMask.GetMask("ground", "monster_main"), true))
        {
            Tornado tornado = Instantiate(tornado_p);
            tornado.transform.position = target;
        }
    }

    // Check the user's input to drive action
    private void checkInput()
    {
        if (Input.GetMouseButton(0))
        {
            strike();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            tornado();
        }
    }

    // Primary attack ability
    private void strike()
    {
        anim.SetTrigger("strike");
        playerMovement.ShouldFaceForward = true;
    }

    // Create a tornado that sucks people in
    private void tornado()
    {
        anim.SetTrigger("tornado");
    }

}
