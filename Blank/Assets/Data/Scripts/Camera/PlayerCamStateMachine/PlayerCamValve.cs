using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamValve : IStates
{
    private PlayerCamera user;
    private Vector3 resetSV;
    public float timer;
    private float timerDefault = 1f;
    public float valueToDesiredPitch;
    private float valueToDesiredYaw;
    public Vector3 target;

    public PlayerCamValve(PlayerCamera user)
    {
        this.user = user;
    }

    public void Enter()
    {
        if (user.stateDebugLog) Debug.Log("sCamReset <color=yellow>Enter<color>");
        if (timer <= 0) timer = timerDefault;
        InputsManager.instance.cameraInputsAreDisabled = true;
    }

    public void IfStateChange()
    {

    }

    public void StateUpdate()
    {
        if (user.stateDebugLog) Debug.Log("sCamReset <color=blue>Update</color>");
        timer -= 1.0f * Time.deltaTime;
        if (timer <= 0)
        {
            if (user.stateDebugLog) Debug.Log("From sReset to sDefault <color=purple>StateChange</color>");
            user.camFSM.ChangeState(user.defaultState);
        }
        user.camTarget.tr.LookAt(target);
        user.pitch = Mathf.Lerp(user.pitch, valueToDesiredPitch, 0.10f);
        user.yaw = Mathf.LerpAngle(user.yaw, user.camTarget.tr.rotation.eulerAngles.y, 0.10f);
        user.Tr.position = Vector3.SmoothDamp(user.Tr.position, user.camTarget.tr.position - user.Tr.forward * user.desiredDollyDst, ref resetSV, 0.025f);
    }

    public void Exit()
    {
        InputsManager.instance.cameraInputsAreDisabled = false;
        if (user.stateDebugLog) Debug.Log("sCamReset <color=yellow>Exit<color>");
    }

}