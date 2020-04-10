using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneLookAround : IStates
{
    private DroneNME user;
    private float tStamp01, t01 = 2.0f, t02 = 6f;
    private bool animDelay, stopLookAroundAnim;
    private bool t01DoOnce, t02DoOnce;
    private float lerpTime;
    private float lightSetupTStamp;
    private float lightSetupDuration = 2f;

    public DroneLookAround(DroneNME user)
    {
        this.user = user;
    }

    public void Enter()
    {
        if (user.stateDebugOn) Debug.Log("droneLookAround <color=yellow>Enter</color>");
        tStamp01 = Time.time;
        t01DoOnce = true;
        t02DoOnce = true;
        lerpTime = 0.0f;
    }

    public void IfStateChange()
    {
        if (user.playerIsVisible)
        {
            if (user.stateDebugOn) Debug.Log("droneLookAround to droneAttack<color=purple>StateChange</color>");
            user.DroneAnimator.SetBool("looking", false);

            Vector3 direction = GameManager.instance.currentAvatar.tr.position - user.tr.position;
            Quaternion desiredRotation = Quaternion.LookRotation(direction, Vector3.up);
            user.tr.rotation = desiredRotation;

            user.Machine.ChangeState(user.DroneAttackChargeUpState);
        }
        else if (lerpTime >= 1)
        {
            if (user.stateDebugOn) Debug.Log("droneLookAround to droneIdle<color=purple>StateChange</color>");
            user.Machine.ChangeState(user.DronePatrolState);
        }
    }

    public void StateUpdate()
    {
        if (user.stateDebugOn) Debug.Log("droneLookAround<color=blue>Update</color>");
        TickStateTimers();

        if (animDelay && t01DoOnce)
        {
            t01DoOnce = false;
            user.DroneAnimator.SetBool("looking", true);
        }

        if (stopLookAroundAnim)
        {
            if(t02DoOnce)
            {
                t02DoOnce = false;
                lightSetupTStamp = Time.time;
                user.DroneAnimator.SetBool("looking", false);
            }
            ResetDroneSpotLight();
        }

        user.DroneUpdateState();
    }

    public void Exit()
    {
        if (user.stateDebugOn) Debug.Log("droneLookAround <color=yellow>Exit</color>");
    }

    #region Functions 

    private void ResetDroneSpotLight()
    {
        lerpTime = (Time.time - lightSetupTStamp) / lightSetupDuration;
        //Debug.Log(" diff : " + (Time.time - lightSetupTStamp) + " dur : " + lightSetupDuration + " lerpTime : " + lerpTime);
        user.spotLight.color = Color.Lerp(user.spotLight.color, user.baseSpotLightColor, lerpTime);
        user.spotLight.spotAngle = Mathf.Lerp(user.spotLight.spotAngle, user.baseLightAngle, lerpTime);
        user.spotLight.intensity = Mathf.Lerp(user.spotLight.intensity, user.baseLightIntensity, lerpTime);
        user.spotLight.bounceIntensity = Mathf.Lerp(user.spotLight.bounceIntensity, user.baseIndirectMult, lerpTime);

        user.spotLightHalo.material.color = Color.Lerp(user.spotLightHalo.material.color, Color.white, lerpTime);
        user.spotLightFlare.material.color = Color.Lerp(user.spotLightFlare.material.color, Color.white, lerpTime);
    }

    private void TickStateTimers()
    {
        animDelay = Time.time - tStamp01 >= t01;
        stopLookAroundAnim = Time.time - tStamp01 >= t02;

    }

    #endregion
}
