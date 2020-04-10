using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DronePatrol : IStates
{
    private DroneNME user;
    private Vector3 currVel;
    public float moveMaxSpd;
    public float moveSmoothTime;

    public DronePatrol(DroneNME user, float moveSmoothTime, float moveMaxSpd)
    {
        bool a = moveMaxSpd <= 0;
        this.user = user;
        this.moveSmoothTime = moveSmoothTime;
        this.moveMaxSpd = moveMaxSpd;
    }

    public void Enter()
    {
        if (user.stateDebugOn) Debug.Log("sPatrol<color=yellow>Enter</color>");
        user.nmeFollow.isFollowing = true;
        user.lerpable.active = true;
        user.DroneAnimator.SetBool("isPatrolling", true);
    }

    public void IfStateChange()
    {
        if (user.playerIsVisible)
        {
            if (user.stateDebugOn) Debug.Log("sPatrol to sChargeUp<color=purple>StateChange</color>");
            user.Machine.ChangeState(user.DroneAttackChargeUpState);
        }
    }

    public void StateUpdate()
    {
        if (user.stateDebugOn) Debug.Log("sPatrol<color=blue>Update</color>");

        user.DroneUpdateState();

        user.transform.position = Vector3.SmoothDamp(user.tr.position, user.nmeFollow.target.position + Vector3.up * user.DroneHeightOffset, ref currVel, moveSmoothTime, moveMaxSpd);

        Vector3 direction = user.nmeFollow.target.position - user.tr.position;
        Quaternion desiredRotation = Quaternion.LookRotation(direction, Vector3.up);
        user.tr.rotation = Quaternion.Lerp(user.tr.rotation, desiredRotation, 0.2f);
    }

    public void Exit()
    {
        if (user.stateDebugOn) Debug.Log("sPatrol<color=yellow>Exit</color>");
        user.nmeFollow.isFollowing = false;
        user.DroneAnimator.SetBool("isPatrolling", false);
    }
}