using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamFall : IStates
{
    private PlayerCamera user;
    private RaycastHit hit;
    private Vector3 dollySV;
    private float smoothTime;


    public PlayerCamFall(PlayerCamera user)
    {
        this.user = user;
    }

    public void Enter()
    {
        if (user.stateDebugLog) Debug.Log("sCamFall <color=yellow>Enter</color>");
        dollySV = Vector3.zero;
        smoothTime = 0.022f;
    }

    public void IfStateChange()
    {
        if (user.isColliding)
        {
            if (user.stateDebugLog) Debug.Log("From sFall to sColl <color=purple>StateChange</color>");
            user.camFSM.ChangeState(user.collisionState);
        }
        else if (user.avatarFSM.currentState != user.avatarFSM.fall)
        {
            if (user.stateDebugLog) Debug.Log("From sFall to sDefault <color=purple>StateChange</color>");
            user.camFSM.ChangeState(user.defaultState);
        }
    }

    public void StateUpdate()
    {
        if (user.stateDebugLog) Debug.Log("sCamFall <color=blue>Update</color>");

        RaisePitchIfObstacle();

        user.Tr.position = Vector3.SmoothDamp(user.Tr.position, user.camTarget.tr.position - user.Tr.forward * user.desiredDollyDst, ref dollySV, smoothTime + 0.003f);
    }

    public void Exit()
    {
        if (user.stateDebugLog) Debug.Log("sCamFall <color=yellow>Exit</color>");
    }

    /// <summary> While falling, check if something directly below might obstrude the camera. If so, attempt to move the cam pitch up preventively. 
    /// This is all made to try to avoid a transition to the collision state while falling, if & when possible</summary>
    private void RaisePitchIfObstacle()
    {
        Vector3 camDownStart = user.Tr.position - (1.6f * user.Tr.up);
        Vector3 camDownEnd = user.camTarget.tr.position - user.Tr.position;
        
        if (Physics.Raycast(camDownStart, camDownEnd, out hit, user.desiredDollyDst, user.ObstaclesLayerMask) && hit.collider.tag != "AllowCameraDissolve")
        {
            Debug.DrawRay(camDownStart, camDownEnd, Color.red);
            user.pitch += 200f * Time.deltaTime;
        } 
        else Debug.DrawRay(camDownStart, camDownEnd, Color.green);
    }
}



