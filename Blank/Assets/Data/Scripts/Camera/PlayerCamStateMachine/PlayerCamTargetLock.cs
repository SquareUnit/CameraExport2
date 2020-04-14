using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamTargetLock : IStates
{
    private PlayerCamera user;
    private Vector3 resetSV;

    private float timerDefault = 0.65f;
    private float valueToDesiredPitch;
    private float valueToDesiredYaw;

    public PlayerCamTargetLock(PlayerCamera user)
    {
        this.user = user;
    }

    public void Enter()
    {
        if (user.stateDebugLog) Debug.Log("sCamReset <color=yellow>Enter<color>");
    }

    public void IfStateChange()
    {
        if (user.stateDebugLog) Debug.Log("From sTargetLock to ****** <color=purple>StateChange</color>");
    }

    public void StateUpdate()
    {
        if (user.stateDebugLog) Debug.Log("sCamReset <color=blue>Update</color>");


        user.pitch = Mathf.Lerp(user.pitch, 15.0f, 0.10f);
        user.yaw = Mathf.LerpAngle(user.yaw, user.camTarget.tr.rotation.eulerAngles.y, 0.10f);
        user.tr.position = Vector3.SmoothDamp(user.tr.position, user.camTarget.tr.position - user.tr.forward * user.desiredDollyDst, ref resetSV, 0.025f);
    }

    public void Exit()
    {
        if (user.stateDebugLog) Debug.Log("sCamReset <color=yellow>Exit<color>");
    }
}
