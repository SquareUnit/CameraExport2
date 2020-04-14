/*Script cree le : 25-04-2019
 * Par: David Babin
 * Derniere modification: 06-05-2019
 * Par: David Babin
 * Description: Activation d'une valve qui fera tourner un pantin de puzzle de Gestalt
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValveTriggerable : Triggerable
{

    public enum Orientation { mural, plancher }
    [Header("Orientation de la valve")]
    public Orientation type;

    /*[HideInInspector]*/
    public PantinGestalt pantinGestalt;
    /*[HideInInspector]*/ public bool isActivate = false;
    [HideInInspector] public Animator myAnimator;
    [HideInInspector] public Transform tr;
    [HideInInspector] public float valveAnimTime = 0.5f;
    private Character avatar;
    [HideInInspector] public bool pantinCamIsOff = false;
    [HideInInspector] public bool puzzleIsDone = false;
    public RefCamera refCamera;
    public GameObject interactButton;
    public SoundRelay soundRelay;
    public bool BlankTrickValveIsUsed = false;

    //FX STEAM
    public ParticleSystem steam;

    private GameObject pantinBouttonValve;
    private bool gotTheButton = false;

    public void Start()
    {
        interactButton.SetActive(false);

        tr = transform;
        myAnimator = GetComponent<Animator>();
        refCamera = GetComponent<RefCamera>();
        soundRelay = GetComponent<SoundRelay>();

        //Steam de la valve
        InvokeRepeating("PlaySteam", 5f, UnityEngine.Random.Range(8f, 10f));

    }

    public void Update()
    {
        avatar = GameManager.instance.currentAvatar;
        if (pantinGestalt == null) pantinGestalt = GetComponent<PantinGestalt>();

        // par philippe
        if (!gotTheButton && pantinGestalt != null)
        {
            pantinBouttonValve = pantinGestalt.bouttonValve;
            gotTheButton = true;
        }

        if (pantinGestalt.myManager != null)
        {
            if (!pantinGestalt.myManager.puzzleCompleted)
            {
                // look at for the button
                if (interactButton.gameObject.activeInHierarchy)
                    interactButton.transform.LookAt(GameManager.instance.currentCamera.tr);

                // test look at bouton temporaire
                if (pantinBouttonValve.activeInHierarchy)
                    pantinBouttonValve.transform.LookAt(GameManager.instance.currentCamera.tr);

                // Afficher le bouton interact (X)
                if (isActivate && avatar.stateMachine.currentState != avatar.stateMachine.interact)
                {
                    ActivateUpdate();
                    interactButton.SetActive(true);
                    pantinBouttonValve.SetActive(true);
                }
                else
                {
                    interactButton.SetActive(false);
                    pantinBouttonValve.SetActive(false);
                }
            }
            else
            {
                interactButton.SetActive(false);
                pantinBouttonValve.SetActive(false);
            }
        }
    }

    public void PlaySteam()
    {
        steam.Play();
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
        if (InputsManager.instance.interact)
        {
            if (avatar.stateMachine.currentState == avatar.stateMachine.crouch)
            {
                avatar.stateMachine.ChangeState(avatar.stateMachine.idle);
            }
            if (avatar.stateMachine.currentState == avatar.stateMachine.idle && avatar.stateMachine.idle.timeInState >= 0.3f)
            {
                if (!BlankTrickValveIsUsed)
                {
                    AnimateAvatar();
                    refCamera.Lookat(pantinGestalt.myPantinTr);
                }
                Invoke("PantinToTurn", 0.6f);
                soundRelay.PlayAudioClip(0);
            }
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

    public void PantinToTurn()
    {
        pantinGestalt.ToNextDirection();
        isActivate = false;
        if(!BlankTrickValveIsUsed)
            Invoke("ResetActivate", 0.5f);
    }

    public void ResetActivate()
    {
        isActivate = true;
    }

    public void PlayFx(AnimationEvent animation)
    {
        GameManager.instance.fxPool.GetObjectAutoReturn(animation.stringParameter, tr.position, tr.eulerAngles, animation.floatParameter);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (pantinGestalt != null)
        {
            Debug.DrawLine(transform.position, pantinGestalt.transform.position);
        }
    }
#endif
}
