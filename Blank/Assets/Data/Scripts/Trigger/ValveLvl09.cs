using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValveLvl09 : Triggerable
{
    public enum Orientation { mural, plancher }
    [Header("Orientation de la valve")]
    public Orientation type;

    public Animator PuzzlePiece;
    public bool isActivate = false;
    public Animator myAnimator;
    public int myState = 0;
    public bool isWinning = false;

    public GestaltManagerLvl9 myManager;

    public GameObject interactButton;

    private Character avatar;
    public Transform tr;
    public float valveAnimTime = 0.5f;

    public RefCamera refCamera;
    public bool isBinaire = false;
    public int bonState;


    // Start is called before the first frame update
    void Start()
    {
        tr = transform;
        interactButton.SetActive(false);

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

    public void Update()
    {
        if (PuzzlePiece == null) PuzzlePiece = GetComponent<Animator>();

        if (myManager != null)
        {
            if (!myManager.win)
            {
                // look at for the button
                if (interactButton.gameObject.activeInHierarchy)
                    interactButton.transform.LookAt(GameManager.instance.currentCamera.Tr);

                // Afficher le bouton interact (X)
                if (isActivate)
                {
                   
                    ActivateUpdate();
                    interactButton.SetActive(true);
                }
                else
                {
                    interactButton.SetActive(false);
                }
            }
            else
            {
               
                interactButton.SetActive(false);
            }
        }

    }

    public void ActivateUpdate()
    {
        avatar = GameManager.instance.currentAvatar;
        if (!isBinaire)
        {

            if (InputsManager.instance.interact && avatar.stateMachine.currentState == avatar.stateMachine.idle && avatar.stateMachine.idle.timeInState >= 0.3f)
            {
                AnimateAvatar();
                refCamera.Lookat(PuzzlePiece.transform);
                myAnimator.SetBool("usedByBlank", true);
                isActivate = false;
                Invoke("ResetActivate", 1f);
                if (myState < 3)
                {
                    myState++;
                    if (myState == bonState)
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
                PuzzlePiece.SetInteger("State", myState);
                myManager.Check();
            }
            else
            {
                myAnimator.SetBool("usedByBlank", false);
            }
        }
        else
        {
            if (InputsManager.instance.interact && avatar.stateMachine.currentState == avatar.stateMachine.idle && avatar.stateMachine.idle.timeInState >= 0.3f)
            {
                AnimateAvatar();
                refCamera.Lookat(PuzzlePiece.transform);
                myAnimator.SetBool("usedByBlank", true);

                if (PuzzlePiece.GetBool("isActive") == false)
                {
                    myAnimator.SetBool("usedByBlank", true);
                    PuzzlePiece.SetBool("isActive", true);
                    isActivate = false;
                    Invoke("ResetActivate", 1f);
                    this.isWinning = true;
                }
                else
                {
                    myAnimator.SetBool("usedByBlank", true);
                    PuzzlePiece.SetBool("isActive", false);
                    isActivate = false;
                    Invoke("ResetActivate", 1f);
                    this.isWinning = false;

                }

                myManager.Check();

            }
            else
            {
                myAnimator.SetBool("usedByBlank", false);
            }
        }
    }

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

    public void ResetActivate()
    {
        isActivate = true;
    }

}
