using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneAttack : IStates
{
    private DroneNME user;
    private Character playerCharacter;
    private Vector3 currVel;

    private bool toResetState;
    private float tStamp01, t01 = 2.0f;

    private float time;
    private Color lazerColor = new Vector4(0.5f, 0f, 1f, 0f);

    public DroneAttack(DroneNME user)
    {
        this.user = user;
    }

    public void Enter()
    {
        if (user.stateDebugOn) Debug.Log("sAttack<color=yellow>Enter</color>");
        user.DroneAnimator.SetTrigger("attack");
        user.soundRelay.PlayAudioClip(0);

        tStamp01 = Time.time;
        toResetState = false;

        playerCharacter = GameManager.instance.currentAvatar; // should not be here, change when redoing level load.
        playerCharacter.animator.SetBool("deathByZap", true);
        playerCharacter.stateMachine.ChangeState(playerCharacter.stateMachine.death);
    }

    public void IfStateChange()
    {
        if (toResetState)
        {
            if (user.stateDebugOn) Debug.Log("sAttack to sReset<color=purple>StateChange</color>");
            user.Machine.ChangeState(user.DroneResetState);
        }
    }

    public void StateUpdate()
    {
        if (user.stateDebugOn) Debug.Log("sAttack<color=blue>Update</color>");

        TickStateTimers();
        user.DroneUpdateState();

        time += 0.05f;
        float alpha = Mathf.Lerp(0.0f, 1.0f, time);
        lazerColor = new Vector4(0.5f, 0.0f, 1.0f, alpha);
        user.droneLazer.startColor = lazerColor;
        user.droneLazer.endColor = lazerColor;

        Vector3 direction = new Vector3(user.avatarTr.position.x, user.avatarTr.position.y + GameManager.instance.avatarHeight / 2, user.avatarTr.position.z) - user.tr.position;
        user.droneLazer.SetPosition(1, direction);
        Quaternion desiredRotation = Quaternion.LookRotation(direction, Vector3.up);
        user.tr.rotation = Quaternion.Lerp(user.tr.rotation, desiredRotation, 0.08f);
    }

    public void Exit()
    {
        if (user.stateDebugOn) Debug.Log("sAttack<color=yellow>Exit</color>");
        user.detectionCone.playerInRange = false;
        user.DroneAnimator.SetBool("isDetectingBlank", false);
        user.droneLazer.enabled = false;

        lazerColor = new Vector4(0.5f, 0f, 1f, 0f);
        user.droneLazer.startColor = lazerColor;
        user.droneLazer.endColor = lazerColor;
    }

    private void TickStateTimers()
    {
        toResetState = Time.time - tStamp01 >= t01;
    }

}