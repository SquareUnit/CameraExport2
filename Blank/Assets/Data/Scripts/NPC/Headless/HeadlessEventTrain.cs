using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadlessEventTrain : IStates
{
    private HeadlessNPC myMaster;


    public void Enter()
    {
        myMaster.agent.enabled = false;
    }

    public void Exit()
    {
        myMaster.animator.SetBool("evt_Train", false);
    }

    public void IfStateChange()
    {

    }

    public void StateUpdate()
    {

    }

    #region Constructor

    public HeadlessEventTrain(HeadlessNPC myMaster)
    {
        this.myMaster = myMaster;
    }
    #endregion
}
