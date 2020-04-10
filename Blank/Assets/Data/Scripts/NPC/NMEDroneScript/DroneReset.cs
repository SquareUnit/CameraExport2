using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneReset : IStates
{
    private DroneNME user;

    private bool toBaseState;
    private float tStamp01, t01 = 3f;

    public DroneReset(DroneNME user)
    {
        this.user = user;
    }

    public void Enter()
    {
        if (user.stateDebugOn) Debug.Log("sReset<color=yellow>Enter</color>");
        tStamp01 = Time.time;
        toBaseState = false;

        ResetDroneAndPath();
        ResetSpotLight();
    }

    public void IfStateChange()
    {
        if (user.baseState == DroneNME.BaseState.idle && toBaseState)
        {
            if (user.stateDebugOn) Debug.Log("sReset to sIdle<color=purple>StateChange</color>");
            user.Machine.ChangeState(user.DroneIdleState);
        }

        if (user.baseState == DroneNME.BaseState.patrol && toBaseState)
        {
            if (user.stateDebugOn) Debug.Log("sReset to sPatrol<color=purple>StateChange</color>");
            user.Machine.ChangeState(user.DronePatrolState);
        }
    }

    public void StateUpdate()
    {
        if (user.stateDebugOn) Debug.Log("sReset<color=blue>Update</color>");
        TickTimers();
    }

    public void Exit()
    {
        if (user.stateDebugOn) Debug.Log("sReset<color=yellow>Exit</color>");
        user.warned = false;
    }

    void ResetDroneAndPath()
    {
        user.tr.position = user.nmeFollow.path.wayPoints[0].tr.position;
        user.nmeFollow.next = user.nmeFollow.path.wayPoints[0];
        user.nmeFollow.target.transform.position = user.nmeFollow.path.wayPoints[0].tr.position;
    }

    void ResetSpotLight()
    {
        user.spotLight.color = user.baseSpotLightColor;
        user.spotLight.spotAngle = user.baseLightAngle;
        user.spotLight.intensity = user.baseLightIntensity;
        user.spotLight.bounceIntensity = user.baseIndirectMult;
        user.spotLightHalo.material.color = Color.white;
        user.spotLightFlare.material.color = Color.white;
    }

    void TickTimers()
    {
        toBaseState = Time.time - tStamp01 >= t01;
    }

}

