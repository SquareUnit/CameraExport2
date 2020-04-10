using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadHunterDoor : IStates
{
    private HeadHunterNME myMaster;
    public Vector3 headHunterDestination;
    public Vector3 headHunterLookAt;
    private bool inAnim;
    private RaycastHit hit;
    public bool placeHH = false;
    public bool canSwitchState = false;
    private float timeInState;


    public void Enter()
    {
        timeInState = 0;
        myMaster.agent.destination = headHunterDestination;
    }

    public void StateUpdate()
    {
        timeInState += Time.deltaTime;

        myMaster.LookForPlayer();
        myMaster.animator.SetBool("isDetectingBlank", myMaster.playerDetected);

        if (myMaster.agent.remainingDistance <= 0.1 && inAnim == false && placeHH)
        {
            Vector3 temp = headHunterLookAt;
            temp.y = myMaster.tr.position.y;
            myMaster.tr.LookAt(temp);
            inAnim = true;
        }

        if (inAnim) myMaster.animator.SetBool("Door", true);

        Physics.Linecast(GameManager.instance.currentAvatar.tr.position, myMaster.tr.position, out hit);
        
        

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("HeadHunter"))
            {
                if (hit.distance <= 1)
                {
                    if (GameManager.instance.currentAvatar.stateMachine.currentState != GameManager.instance.currentAvatar.stateMachine.death)
                    {
                        GameManager.instance.currentAvatar.animator.SetBool("deathByTrap", true);
                        GameManager.instance.currentAvatar.stateMachine.ChangeState(GameManager.instance.currentAvatar.stateMachine.death);
                    }
                }
                else
                {
                    if(hit.distance + 1 <= 10)
                    {
                        if (timeInState >= 2)
                        {
                            ExitState();
                        }
                    }
                }
            }
        }
        
        if(timeInState >= 4)
        {
            ExitState();
        }
    }

    public void Exit()
    {

    }

    public void IfStateChange()
    {

    }

    private void ExitState()
    {
        placeHH = false;
        myMaster.animator.SetBool("Door", false);
        inAnim = false;
        myMaster.stateMachine.ChangeState(myMaster.patrol);
    }

    #region Constructor

    public HeadHunterDoor(HeadHunterNME myMaster)
    {
        this.myMaster = myMaster;
    }

    #endregion
}
