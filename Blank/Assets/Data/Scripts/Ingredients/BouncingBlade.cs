using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingBlade : MonoBehaviour
{

    [HideInInspector] public Collider exteriorTrigger;
    [HideInInspector] public Collider interiorTrigger;

    [HideInInspector] public Trigger trigE;
    [HideInInspector] public Trigger trigI;

    [HideInInspector] public Animator animator;
    [HideInInspector] public Animator animatorFX;

    private bool isDead = false;
    [HideInInspector] public bool isInExtTrigger = false;
    [HideInInspector] public bool isInIntTrigger = false;

    public SoundRelay soundRelay;

    private BoxCollider collisionBB;

    private float delaiTemps = 0.83f; // 15 frames of reset position + 10 frames pour le cycle de l'anim, total  0.83 secondes

    private float posCheckBB;

    private Transform avatarTR;

    public bool alwaysON = false;

    private void Start()
    {
        soundRelay = GetComponent<SoundRelay>();
        trigE = exteriorTrigger.GetComponent<Trigger>();
        trigI = interiorTrigger.GetComponent<Trigger>();

        posCheckBB = transform.position.y - 0.5f;

        collisionBB = GetComponentInChildren<BoxCollider>();

        if (alwaysON)
        {
            animator.SetBool("detect", true);
            Invoke("TriggerTrap", 2.0f);
        }
    }

    private void Update()
    {
        if (GameManager.instance.currentAvatar != null) // lorsque l'avatar est présent
        {
            avatarTR = GameManager.instance.currentAvatar.transform;

            // next line null reference ?
            if (GameManager.instance.currentAvatar != null && GameManager.instance.currentAvatar.stateMachine.currentState == GameManager.instance.currentAvatar.stateMachine.death) // check si l'avatar est en Death State
            {
                isDead = true;
                // Reset Trigger Detection
                isInExtTrigger = false;
                isInIntTrigger = false;
            }
            else
            {
                isDead = false;
            }
        }
    }

    public void Detect()
    {
        //Detect if the player is between 2 and 3 meters
        if (isInExtTrigger)
        {
            animator.SetBool("detect", true);
        } 
    }


    public void TriggerTrap()
    {
        if (isInIntTrigger || alwaysON)
        {
            animator.SetBool("attack", true);
            soundRelay.PlayAudioClip(0);
            Invoke("TimingFX", 0.26f);;
            Invoke("Attack", 0.5f); // 15 frames    
        }
    }

    // A little hack to time the FX!
    public void TimingFX()
    {
        animatorFX.SetTrigger("attack");
    }

    public void Attack()
    {
        if (GameManager.instance.currentAvatar != null) // reference null
        {
            // DEATH
            if (!alwaysON && !GameManager.instance.currentAvatar.isCrouching || (!GameManager.instance.currentAvatar.onGround && isInIntTrigger && isInExtTrigger))
            {
                animator.SetBool("attack", false);

                if (GameManager.instance.currentAvatar.stateMachine.currentState != GameManager.instance.currentAvatar.stateMachine.death &&
                posCheckBB <= GameManager.instance.currentAvatar.tr.position.y && isInIntTrigger && isInExtTrigger) // check if player is under the bouncing blade
                {
                    DeathToBlank();
                }
            }


            // Exception for death
            if (!alwaysON && avatarTR.position.y >= collisionBB.bounds.max.y && GameManager.instance.currentAvatar.isCrouching) // check if avatar crouch on top
            {
                if (avatarTR.position.x < collisionBB.transform.position.x + 0.2 || avatarTR.position.x < collisionBB.transform.position.x - 0.2 && avatarTR.position.z < collisionBB.transform.position.z + 0.2 || avatarTR.position.x < collisionBB.transform.position.z - 0.2)
                {
                    DeathToBlank();
                    //Debug.Log("U dead");
                }
            }


            // Trigger if crouch
            if (!isDead && isInIntTrigger && isInExtTrigger)
            {
                Invoke("TriggerTrap", delaiTemps);
            }
        }

        // Continuous BB
        if (alwaysON)
        {
            Invoke("TriggerTrap", delaiTemps);
        }

        // need to check for player in 0.5f sec
        animator.SetBool("attack", false);
        //Debug.Log("hehjen,df");

    }

    public void DeathToBlank()
    {
        GameManager.instance.currentAvatar.ExpediteHead(transform);
        GameManager.instance.currentAvatar.animator.SetBool("deathByTrap", true);
        GameManager.instance.currentAvatar.stateMachine.ChangeState(GameManager.instance.currentAvatar.stateMachine.death);

        // Reset the bouncing blade 
        animator.SetBool("detect", false); // reset the bouncing blade
    }
}
