using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamDefault : IStates
{
    private PlayerCamera user;
    private Vector3 dollySV;
    private float smoothTime;
    private float raisePitchSpd;
    private float lowerPitchSpd;

    public PlayerCamDefault(PlayerCamera user)
    {
        this.user = user;
    }

    public void Enter()
    {
        PrintStateLog(0);
        lowerPitchSpd = 13f;
        dollySV = Vector3.zero;
        smoothTime = 0.022f;
    }

    public void IfStateChange()
    {
        if (InputsManager.instance.camButton)
        {
            PrintStateLog(1);
            user.camFSM.ChangeState(user.resetState);
        }
        else if (user.isColliding)
        {
            PrintStateLog(2);
            user.camFSM.ChangeState(user.collisionState);
        }
        else if (GameManager.instance.currentAvatar.velocityY < 0)
        {
            PrintStateLog(3);
            user.camFSM.ChangeState(user.fallingState);
        }
    }

    public void StateUpdate()
    {
        PrintStateLog(4);

        if (user.camFSM.previousState == user.fallingState) RestoreDefaultPitch();
        RaisePitchWhileMoving();

        user.Tr.position = Vector3.SmoothDamp(user.Tr.position, user.camTarget.tr.position - user.Tr.forward * user.desiredDollyDst, ref dollySV, smoothTime);
    }

    public void Exit()
    {
        PrintStateLog(5);
    }

    /// <summary> Slowly lower the camera pitch after falling downward and while in movement
    /// Counter balance a function in the fall state that raise the pitch upward</summary>
    private void RestoreDefaultPitch()
    {
        if (InputsManager.instance.leftStick != new Vector3(0, 0, 0) && InputsManager.instance.rightStick == new Vector3(0, 0, 0))
        {
            if (user.pitch > 25.0f && lowerPitchSpd > 0)
            {
                lowerPitchSpd -= 0.06f;
                if(lowerPitchSpd < 4.0f) lowerPitchSpd = 4.0f;
                user.pitch -= lowerPitchSpd * Time.deltaTime;
            }
        }
    }

    /// <summary> Raise the camera pitch when moving while the camera close to the avatar and looking upward.
    /// This happend when the player lower the camera to look up/around and start moving again</summary>
    private void RaisePitchWhileMoving()
    {
        if (user.pitch < 15 && InputsManager.instance.rightStick.y > 0)
        {
            raisePitchSpd += 90.0f * Time.deltaTime;
            if (raisePitchSpd >= 800) raisePitchSpd = 800;
        }

        if (InputsManager.instance.leftStick != new Vector3(0, 0, 0) && InputsManager.instance.rightStick == new Vector3(0, 0, 0))
        {
            if (user.pitch < 15)
            {
                raisePitchSpd *= 0.962f;
                user.pitch += raisePitchSpd * Time.deltaTime;
            }
        }
    }

    // TODO: Change to proceduraly generated logs using camFSM previous, current and next state.
    private void PrintStateLog(int a)
    {
        if (user.stateDebugLog)
        {
            switch (a)
            {
                case 0:
                    Debug.Log("sCamDefault <color=yellow>Enter</color>");
                    break;
                case 1:
                    Debug.Log("From sDefault to sReset <color=purple>StateChange</color>");
                    break;
                case 2:
                    Debug.Log("From sDefault to sColl <color=purple>StateChange</color>");
                    break;
                case 3:
                    Debug.Log("From sDefault to sFall <color=purple>StateChange</color>");
                    break;
                case 4:
                    Debug.Log("sCamDefault <color=blue>Update</color>");
                    break;
                case 5:
                    Debug.Log("sCamDefault <color=yellow>Exit</color>");
                    break;
                default:
                    Debug.Log("Missing log");
                    break;

            }
        }
    }

}