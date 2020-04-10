﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadHunterPatrol : IStates
{
    private HeadHunterNME myMaster;
    private float angle;

    #region Constructor

    public HeadHunterPatrol(HeadHunterNME myMaster)
    {
        this.myMaster = myMaster;
    }

    #endregion

    public void Enter()
    {
        myMaster.animator.SetBool("isPatrolling", true);

        if (myMaster.stateMachine.previousState != myMaster.turn)
        {
            //Retourne sur son Path ou sur sa position de départ
            if (myMaster.pathObject != null)
                myMaster.agent.SetDestination(myMaster.path[myMaster.CheckClosestDestination()].position);
            else
                myMaster.agent.SetDestination(myMaster.startPos);
        }
    }

    public void IfStateChange()
    {

    }

    public void StateUpdate()
    {

        angle = Vector3.SignedAngle(myMaster.tr.forward, myMaster.agent.destination - myMaster.tr.position, myMaster.tr.up);
        myMaster.animator.SetFloat("turn", angle);

        if (myMaster.pathObject != null)
        {
            
            if (myMaster.agent.remainingDistance <= myMaster.distanceToTurn)
            {
                myMaster.UpdatePath();
                myMaster.agent.SetDestination(myMaster.path[myMaster.pathCpt].position);
                angle = Vector3.SignedAngle(myMaster.tr.forward, myMaster.agent.destination - myMaster.tr.position, myMaster.tr.up);
                if (angle >= myMaster.turnThreshold || angle <= -myMaster.turnThreshold)
                    myMaster.stateMachine.ChangeState(myMaster.turn);
            }
        }
        else
        {
            if (myMaster.agent.remainingDistance <= 0.1)
            {
                myMaster.tr.SetPositionAndRotation(myMaster.startPos, myMaster.startRot);
                myMaster.stateMachine.ChangeState(myMaster.idle);
                return;
            }
        }

        myMaster.LookForPlayer();
        myMaster.animator.SetBool("isDetectingBlank", myMaster.playerDetected);

        if (myMaster.playerDetected)
            myMaster.stateMachine.ChangeState(myMaster.chase);
    }
    public void Exit()
    {
        myMaster.agent.isStopped = true;
        myMaster.animator.SetBool("isPatrolling", false);
    }
}