using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalGirlController : MonoBehaviour
{

    public ParticleSystem clawEffect_p; // The particle for the claw effect
    public Transform clawEffectLocation;    // Location to spawn the claw attack effect

    private Animator anim;
    private PlayerMovement playerMovement;   // Controls the player movement

    // Called from animation
    public void Claw()
    {
        ParticleSystem clawEffect = Instantiate(clawEffect_p);
        clawEffect.transform.position = clawEffectLocation.transform.position;

        Destroy(clawEffect.gameObject, 2.0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();

        Debug.Assert(anim);
        Debug.Assert(playerMovement);
    }

    // Update is called once per frame
    void Update()
    {
        checkInput();        
    }

    // Check input for abilities
    private void checkInput()
    {
        // Claw
        if (Input.GetMouseButtonDown(0))
        {
            claw();
        }
    }

    // Cast the claw ability
    private void claw()
    {
        anim.SetTrigger("claw");
        playerMovement.ShouldFaceForward = true;
    }

}
