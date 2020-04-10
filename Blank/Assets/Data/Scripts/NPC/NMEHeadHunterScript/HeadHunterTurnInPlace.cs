using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadHunterTurnInPlace : IStates
{
    private HeadHunterNME myMaster;
    private float angle;
    private Vector3 currentVelocity;
    private bool turningRight;

    public void Enter()
    {
        myMaster.agent.isStopped = true;
        myMaster.agent.updateRotation = false;
        angle = Vector3.SignedAngle(myMaster.tr.forward, myMaster.agent.destination - myMaster.tr.position, myMaster.tr.up);
        if (angle < 0f)
        {
            myMaster.animator.SetBool("turninPlace_L", true);
            turningRight = false;
        }
        else
        {
            myMaster.animator.SetBool("turninPlace_R", true);
            turningRight = true;
        }
    }

    public void StateUpdate()
    {
        angle = Vector3.SignedAngle(myMaster.tr.forward, myMaster.agent.destination - myMaster.tr.position, myMaster.tr.up);

        if (turningRight && angle <= 0f)
            myMaster.stateMachine.ChangeState(myMaster.patrol);
        else if (!turningRight && angle >= 0f)
            myMaster.stateMachine.ChangeState(myMaster.patrol);

        myMaster.LookForPlayer();
        myMaster.animator.SetBool("isDetectingBlank", myMaster.playerDetected);

        if (myMaster.playerDetected)
            myMaster.stateMachine.ChangeState(myMaster.chase);
    }

    public void Exit()
    {
        myMaster.agent.isStopped = false;
        myMaster.agent.updateRotation = true;
        myMaster.animator.SetBool("turninPlace_L", false);
        myMaster.animator.SetBool("turninPlace_R", false);
    }

    public void IfStateChange()
    {

    }


    #region Constructor

    public HeadHunterTurnInPlace(HeadHunterNME myMaster)
    {
        this.myMaster = myMaster;
    }

    #endregion
}
