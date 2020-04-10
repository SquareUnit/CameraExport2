using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerCamRevealInfo))]
public class CamLookTriggerable : Triggerable
{
    public enum StateVoulu { CamReveal, CamDefault, CamReset, CamFall, CamCollision }
    public StateVoulu stateVoulu;
    private StateMachine camFSM;
    private PlayerCamera currentCamera;
    private bool hasWaitedAfterLoad = false;
    private bool canSetup1 = true;
    private bool canSetup2 = true;
    bool enActivation = false;

    private void Awake()
    {
        Invoke("WaitAfterLoad", 1.0f);
    }

    private void WaitAfterLoad()
    {
        hasWaitedAfterLoad = true;
    }

    private void Update()
    {
        if(hasWaitedAfterLoad)
        {
            if (GameManager.instance.currentCamera.camFSM != null && canSetup1)
            {
                camFSM = GameManager.instance.currentCamera.camFSM;
                canSetup1 = false;
            }
            if (GameManager.instance.currentCamera != null && canSetup2)
            {
                currentCamera = GameManager.instance.currentCamera;
                canSetup2 = false;
            }
        }
    }

    public override void Activate()
    {
        if(enActivation == false)
        {
            ChangeState();
        }
    }

    public override void Deactivate()
    {
        if (enActivation == false)
        {
            ChangeState();
        }
    }

    public override void Toggle()
    {
        if (enActivation == false)
        {
            ChangeState();
        }
    }

    public void ChangeState()
    {
        enActivation = true;

        switch((int)stateVoulu)
        {
            case 0:
                if (GetComponent<PlayerCamRevealInfo>() != null)
                {
                    GameManager.instance.currentCamera.revealState.info = GetComponent<PlayerCamRevealInfo>();
                    camFSM.ChangeState(currentCamera.revealState);
                }
                else Debug.LogError("You need to drag a game object to the camRevealInfo script");
                break;

            case 1:
                camFSM.ChangeState(currentCamera.defaultState);
                break;

            case 2:
                camFSM.ChangeState(currentCamera.resetState);
                break;

            case 3:
                camFSM.ChangeState(currentCamera.fallingState);
                break;

            case 4:
                camFSM.ChangeState(currentCamera.collisionState);
                break;
        }

        enActivation = false;
    }
}


