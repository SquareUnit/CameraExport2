using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDeath : IStates
{
    private Character myMaster;
    private CharacterState stateMachine;
    private float timeInState;
    private float timeToExit = 3f;

    public void Enter()
    {
        GameManager.instance.characterIsDead = true;
        timeInState = 0f;
        InputsManager.instance.SetVibration(1f, 1.5f, true);
        myMaster.soundRelay.PlayAudioClip(7);
        if(myMaster.killedByHh == false)
            UIManager.instance.Invoke("EnableGameOver", 1f);
        myMaster.killedByHh = false;
    }

    public void StateUpdate()
    {
        timeInState += Time.deltaTime;

        if(timeInState >= timeToExit)
            stateMachine.ChangeState(stateMachine.idle);

        if (!myMaster.onGround)
            myMaster.MoveCharacter(Vector3.zero);
    }

    public void Exit()
    {
        myMaster.blankHead.enabled = true;
        myMaster.animator.SetBool("deathByTrap", false);
        myMaster.animator.SetBool("deathByZap", false);
        myMaster.canDecrouch = true;
        myMaster.isCrouching = false;
        InputsManager.instance.cameraInputsAreDisabled = false;

        GameManager.instance.CallResetOnDeath();
        GameManager.instance.RespawnAtCheckpoint();
        GameManager.instance.cameraManager.thirdPersonCam.camTarget.tr.position = myMaster.tr.position;
    }

    #region Constructor
    public CharacterDeath(Character myMaster, CharacterState stateMachine)
    {
        this.myMaster = myMaster;
        this.stateMachine = stateMachine;
    }
    #endregion

    public void IfStateChange()
    {

    }
}
