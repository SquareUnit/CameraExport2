using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValvesLvl9Section2_2 : Triggerable {
    public Animator AnimatorPuzzlePiece2;
    public bool isActivate = false;
    public Animator myAnimator;
    public int myState = 0;
    public bool isWinning = false;

    public GestaltManagerLvl9 myManager;

    public GameObject interactButton;

    private Character avatar;
    public Transform tr;
    public float valveAnimTime = 0.5f;

    public void Start()
    {
        tr = transform;
        interactButton.SetActive(false);
    }

    public void Update()
    {
        if (isActivate == true)
        {
            ActivateUpdate();
        }

        //look at for the button
        if (interactButton.gameObject.activeInHierarchy) { interactButton.transform.LookAt(GameManager.instance.currentCamera.Tr); }

        //Afficher le bouton interact(X)
        if (isActivate)
        {
            //affiche bouton
            interactButton.SetActive(true);

        }
        else interactButton.SetActive(false);
    }

    public override void Activate()
    {
        isActivate = true;
    }

    public override void Deactivate()
    {

        isActivate = false;
    }

    public override void Toggle()
    {

    }

    public void ActivateUpdate()
    {

        avatar = GameManager.instance.currentAvatar;


        if (InputsManager.instance.interact && avatar.stateMachine.currentState == avatar.stateMachine.idle && avatar.stateMachine.idle.timeInState >= 0.3f)
        {
            AnimateAvatar();
            myAnimator.SetBool("usedByBlank", true);

            isActivate = false;
            Invoke("ResetActivate", 1f);
            if (myState < 3)
            {
                myState++;
                if (myState == 2)
                {
                    this.isWinning = true;
                }
                else
                    this.isWinning = false;
            }
            else
            {
                myState = 1;
                this.isWinning = false;
            }

            AnimatorPuzzlePiece2.SetInteger("State", myState);
            myManager.Check();

        }
        else
        {
            myAnimator.SetBool("usedByBlank", false);
        }       
    }

    public void ResetActivate()
    {
        isActivate = true;
    }

    public void AnimateAvatar()
    {
        //State interaction de l'avatar
        avatar.stateMachine.interact.interactTime = 1.8f;
        avatar.stateMachine.ChangeState(avatar.stateMachine.interact);

        //Rotation
        avatar.tr.LookAt(tr.position, Vector3.up);

        //Position
        avatar.tr.position = tr.position;
        avatar.tr.position += -avatar.tr.forward * 0.7f;

        avatar.animator.SetTrigger("valveGround");
        Invoke("PlayValveAnim", valveAnimTime);

    }
}