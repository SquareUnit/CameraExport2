using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamOnLoad : IStates
{
    private PlayerCamera user;
    private float tStamp;
    private float timer;

    public PlayerCamOnLoad(PlayerCamera user)
    {
        this.user = user;
    }

    public void Enter()
    {
        if (user.stateDebugLog) Debug.Log("sOnLoad <color=yellow>Enter</color>");
        tStamp = Time.time;
        timer = 2.5f;
    }

    public void IfStateChange()
    {

    }

    public void StateUpdate()
    {
        if(Time.time - tStamp > timer)
        {
            user.camTarget.tr.position = GameManager.instance.currentAvatar.tr.position;
            user.Tr.position = GameManager.instance.currentAvatar.tr.position - user.Tr.forward * user.desiredDollyDst;
            if (user.stateDebugLog) Debug.Log("From sOnLoad to sDefault <color=purple>StateChange</color>");
            user.camFSM.ChangeState(user.defaultState);
            
        }
    }

    public void Exit()
    {
        if (user.stateDebugLog) Debug.Log("sOnLoad <color=yellow>Exit</color>");
    }
}
