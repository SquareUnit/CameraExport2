using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAttackChargeUp : IStates
{
    private DroneNME user;
    private Vector3 currVel;
    private Vector3 lookDir;
    private Quaternion desiredRotation;

    private float chargeUpTStamp, chargeUpDuration = 2.2f;
    private bool chargeUpIsDone;

    private float lerpTime, lightSetupTStamp, lightSetupDuration = 2.2f;

    public DroneAttackChargeUp(DroneNME user)
    {
        this.user = user;
    }

    public void Enter()
    {
        if (user.stateDebugOn) Debug.Log("sChargeUp<color=yellow>Enter</color>");
        InputsManager.instance.SetVibration(0.05f);
        user.soundRelay.PlayAudioClip(1);
        user.DroneAnimator.SetBool("isDetectingBlank", true);
        chargeUpTStamp = Time.time;
        lightSetupTStamp = Time.time;
        lerpTime = 0.0f;
    }

    public void IfStateChange()
    {
        if (user.playerIsVisible && chargeUpIsDone)
        {
            if (user.stateDebugOn) Debug.Log("sChargeUp to sAttack<color=purple>StateChange</color>");
            user.Machine.ChangeState(user.DroneAttackState);
        }
        else if (!user.playerIsVisible && chargeUpIsDone
            || !GameManager.instance.currentAvatar.controller.enabled && chargeUpIsDone
            || GameManager.instance.currentAvatar.stateMachine.currentState == GameManager.instance.currentAvatar.stateMachine.death && chargeUpIsDone)
        {
            if (user.stateDebugOn) Debug.Log("sChargeUp to sIdle<color=purple>StateChange</color>");
            user.DroneAnimator.SetBool("isDetectingBlank", false);
            user.Machine.ChangeState(user.DroneLookAroundState);
        }
    }

    public void StateUpdate()
    {
        if (user.stateDebugOn) Debug.Log("sChargeUp<color=blue>Update</color>");

        TickStateTimers();
        user.DroneUpdateState();
        SetDroneOrientation();
        SetSpotLightParams();
    }

    public void Exit()
    {
        InputsManager.instance.StopVibration();
        if (user.stateDebugOn) Debug.Log("sChargeUp<color=yellow>Exit</color>");
    }

    #region Functions

    private void SetDroneOrientation()
    {
        if (user.playerIsVisible)
        {
            lookDir = new Vector3(user.avatarTr.position.x, user.avatarTr.position.y + GameManager.instance.avatarHeight / 2, user.avatarTr.position.z) - user.tr.position;
            desiredRotation = Quaternion.LookRotation(lookDir, Vector3.up);
            user.tr.rotation = Quaternion.Lerp(user.tr.rotation, desiredRotation, 0.08f);
        }
    }

    private void SetSpotLightParams()
    {
        lerpTime = (Time.time - lightSetupTStamp) / lightSetupDuration;
        user.spotLight.color = Color.Lerp(user.baseSpotLightColor, user.targetSpotLightColor, lerpTime);
        user.spotLight.spotAngle = Mathf.Lerp(user.baseLightAngle, user.targetLightAngle, lerpTime);
        user.spotLight.intensity = Mathf.Lerp(user.baseLightIntensity, user.targetLightIntensity, lerpTime);
        user.spotLight.bounceIntensity = Mathf.Lerp(user.baseIndirectMult, user.targetIndirectMult, lerpTime);

        user.spotLightHalo.material.color = Color.Lerp(Color.white, user.targetHaloColor, lerpTime);
        user.spotLightFlare.material.color = Color.Lerp(Color.white, user.targetFlareColor, lerpTime);
    }

    void TickStateTimers()
    {
        chargeUpIsDone = Time.time - chargeUpTStamp >= chargeUpDuration;
    }

    #endregion
}
