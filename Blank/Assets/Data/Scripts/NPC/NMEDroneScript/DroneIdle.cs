using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneIdle : IStates
{
    private DroneNME user;

    public float tStamp, duration;
    private bool timerUp;
    public bool lookAround, patrol;

    public DroneIdle(DroneNME user)
    {
        this.user = user;
    }

    public void Enter()
    {
        if (user.stateDebugOn) Debug.Log("sDroneIdle<color=yellow>Enter</color>");
        tStamp = Time.time;
        timerUp = false;
        user.nmeFollow.isFollowing = false;
        user.lerpable.active = false;
    }

    public void IfStateChange()
    {
        if (user.playerIsVisible)
        {
            if (user.stateDebugOn) Debug.Log("sIdle to sLookAround<color=purple>StateChange</color>");
            user.Machine.ChangeState(user.DroneAttackChargeUpState);
        }
        else if (patrol && timerUp && user.nmeFollow.path != null)
        {
            if (user.stateDebugOn) Debug.Log("sIdle to sPatrol<color=purple>StateChange</color>");
            user.Machine.ChangeState(user.DronePatrolState);
        }
        else if (lookAround && timerUp)
        {
            if (user.stateDebugOn) Debug.Log("sIdle to sAttack<color=purple>StateChange</color>");
            user.Machine.ChangeState(user.DroneLookAroundState);
        }
    }

    public void StateUpdate()
    {
        if (user.stateDebugOn) Debug.Log("sIdle<color=blue>Update</color>");

        TickTimers();
        user.DroneUpdateState();
    }

    public void Exit()
    {
        if (user.stateDebugOn) Debug.Log("sDroneIdle<color=yellow>Exit</color>");
    }

    private void TickTimers()
    {
        timerUp = Time.time - tStamp >= duration;
    }
}