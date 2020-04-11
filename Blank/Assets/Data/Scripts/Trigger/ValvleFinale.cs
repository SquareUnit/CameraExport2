using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ValvleFinale : Triggerable
{
    public enum Orientation { mural, plancher }
    [Header("Orientation de la valve")]
    public Orientation type;
   
    [HideInInspector]
    public bool isActivate = false;
    [HideInInspector]
    public Animator myAnimator;
    [HideInInspector]
    public Transform tr;
    [HideInInspector]
    public float valveAnimTime = 0.5f;
    private Character avatar;
    [HideInInspector]
    public bool pantinCamIsOff = false;
    [HideInInspector]
    public bool puzzleIsDone = false;
    public RefCamera refCamera;
    public GameObject interactButton;
    public bool BlankTrickValveIsUsed = false;
    public Triggerable monTriggerrable;

    public void Start()
    {
        interactButton.SetActive(false);

        tr = transform;
        myAnimator = GetComponent<Animator>();
        refCamera = GetComponent<RefCamera>();
       
    }

    public void Update()
    {
        // look at for the button
        if (interactButton.gameObject.activeInHierarchy) { interactButton.transform.LookAt(GameManager.instance.currentCamera.Tr); }

        // Afficher le bouton interact (X)
        if (isActivate)
        {
            //affiche bouton
            interactButton.SetActive(true);

        }
        else
        {
            interactButton.SetActive(false);
        }
    }

    public void LateUpdate()
    {
        if (isActivate == true)
        {
            ActivateUpdate();
        }
    }

    public override void Activate()
    {
        //GameManager.instance.pantinCamIsOff = true;
        isActivate = true;
    }

    public override void Deactivate()
    {
        //GameManager.instance.pantinCamIsOff = false;
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
            if (!BlankTrickValveIsUsed)
            {
                AnimateAvatar();
                refCamera.Lookat(monTriggerrable.transform);
            }
            Invoke("Rotate", 0.6f);
        }
        else
        {
            myAnimator.SetBool("usedByBlank", false);
        }

    }

    /// <summary> Active interaction dans l'avatar </summary>
    private void AnimateAvatar()
    {
        //State interaction de l'avatar
        avatar.stateMachine.interact.interactTime = 1.8f;
        avatar.stateMachine.ChangeState(avatar.stateMachine.interact);

        if (type == Orientation.plancher)
        {
            //Rotation
            avatar.tr.LookAt(tr.position, Vector3.up);

            //Position
            avatar.tr.position = tr.position;
            avatar.tr.position += -avatar.tr.forward * 0.7f;


            avatar.animator.SetTrigger("valveGround");
            Invoke("PlayValveAnim", valveAnimTime);
        }
        else
        {
            //Position
            avatar.tr.position = tr.position + Vector3.down * 0.95f + tr.up * 1.25f;

            //Rotation
            Vector3 lookAtPos = tr.position;
            lookAtPos.y = avatar.tr.position.y;
            avatar.tr.LookAt(lookAtPos);


            avatar.animator.SetTrigger("valveWall");
            Invoke("PlayValveAnim", valveAnimTime);
        }
    }

    public void PlayValveAnim()
    {
        myAnimator.SetBool("usedByBlank", true);
    }

    public void Rotate()
    {
        monTriggerrable.Activate();
        isActivate = false;
        Invoke("ResetActivate", 1f);
    }

    public void ResetActivate()
    {
        isActivate = true;
    }

    public void PlayFx(AnimationEvent animation)
    {
        GameManager.instance.fxPool.GetObjectAutoReturn(animation.stringParameter, tr.position, tr.eulerAngles, animation.floatParameter);
    }

}
