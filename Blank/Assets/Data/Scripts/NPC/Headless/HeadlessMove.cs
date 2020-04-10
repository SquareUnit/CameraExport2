using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadlessMove : IStates
{
    private HeadlessNPC user; // =master
    private float angle;
    private Vector3 lastFramePos;

    #region CONSTRUCTOR
    /// <summary> Fire the constructor of a state. In this state, the drone stays immobile, can detect & look around at interval if desired </summary>
    /// <param name="user"> The drone instance using the state machine</param>
    public HeadlessMove(HeadlessNPC user)
    {
        this.user = user;
    }
    #endregion

    public void Enter()
    {
            user.animator.SetBool("isPatrolling", true);
    }

    public void IfStateChange()
    {

    }

    public void StateUpdate()
    {
        angle = Vector3.SignedAngle(user.tr.forward, user.agent.destination - user.tr.position, user.tr.up);
        user.animator.SetFloat("turn", angle);

        if (user.pathObject != null && user.agent.remainingDistance <= user.distanceToTurn)
        {
            user.UpdatePath();
            user.agent.SetDestination(user.path[user.pathCpt].position);
        }
    }
    public void Exit()
    {
        user.animator.SetBool("isPatrolling", false);
        user.animator.SetBool("isRunning", false);
    }
}
