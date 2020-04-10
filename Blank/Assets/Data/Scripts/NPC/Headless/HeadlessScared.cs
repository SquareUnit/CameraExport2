using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadlessScared : IStates
{

    private HeadlessNPC user; // =master

    #region CONSTRUCTOR
    /// <summary> Fire the constructor of a state. In this state, the drone stays immobile, can detect & look around at interval if desired </summary>
    /// <param name="user"> The drone instance using the state machine</param>
    public HeadlessScared(HeadlessNPC user)
    {
        this.user = user;
    }
    #endregion

    public void Enter()
    {
        user.animator.SetBool("isScared", true);
    }

    public void IfStateChange()
    {

    }

    public void StateUpdate()
    {

    }

    public void Exit()
    {

    }

}
