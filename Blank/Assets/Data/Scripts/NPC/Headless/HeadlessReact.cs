using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadlessReact : IStates
{
    private HeadlessNPC user; // =master

    private float timeInState;
    public float timeToExit = 5f;

    #region CONSTRUCTOR
    /// <summary> Fire the constructor of a state. In this state, the drone stays immobile, can detect & look around at interval if desired </summary>
    /// <param name="user"> The drone instance using the state machine</param>
    public HeadlessReact(HeadlessNPC user)
    {
        this.user = user;

    }
    #endregion

    public void Enter()
    {
        timeInState = 5.0f;
        Vector3 avatarPos = GameManager.instance.currentAvatar.tr.position;
        avatarPos.y = user.transform.position.y;
        user.tr.LookAt(avatarPos);
    }

    public void IfStateChange()
    {

    }

    public void StateUpdate()
    {
        timeInState += Time.deltaTime;

        //user.tr.LookAt(GameManager.instance.currentAvatar.tr);

        if (timeInState >= timeToExit)
        {
            if (InputsManager.instance.jump || InputsManager.instance.triggerLeftDown || InputsManager.instance.triggerRightDown) 
            {
                // need to prevent spam inputs
                user.RandomReact();
                timeInState = 0.0f;
            }
        }


    }
    public void Exit()
    {

    }

}
