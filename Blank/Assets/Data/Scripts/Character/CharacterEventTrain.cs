using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEventTrain : IStates
{
    private Character myMaster;
    private CharacterState stateMachine;
    private RaycastHit hit;
    private int inputs = 0;
    public bool canInput = true;

    public void Enter()
    {
        myMaster.controller.enabled = true;
        myMaster.animator.SetLayerWeight(3, 1f);
        myMaster.animator.SetBool("evt_train", true);
        inputs = 0;
    }

    public void StateUpdate()
    {
        if (InputsManager.instance.leftStick.x > 0f || InputsManager.instance.crouch || InputsManager.instance.triggerRight)
        {
            if (canInput)
            {
                inputs += 1;
                canInput = false;
                myMaster.Invoke("ResetCanInput", 1f);
                InputsManager.instance.SetVibration(0.2f, 0.5f, false);
                myMaster.animator.SetTrigger("input_R");
                GameManager.instance.callAdditiveStart.Invoke();
            }
        }

        if (InputsManager.instance.leftStick.x < 0f || InputsManager.instance.jump || InputsManager.instance.triggerLeft)
        {
            if (canInput)
            {
                inputs += 1;
                canInput = false;
                myMaster.Invoke("ResetCanInput", 1f);
                InputsManager.instance.SetVibration(0.2f, 0.5f, false);
                myMaster.animator.SetTrigger("input_L");
                GameManager.instance.callAdditiveStart.Invoke();
            }
        }

        if (inputs >= 6)
        {
            myMaster.animator.SetBool("isWalking", false);
            InputsManager.instance.SetVibration(1f, 1f, true);
            myMaster.stateMachine.ChangeState(myMaster.stateMachine.idle);
            myMaster.headlessInBack.NextPosInLine();

            myMaster.headlessInFront.isInFrontOfBlank = false;
            myMaster.headlessInFront.eventFileSuivant = myMaster.headlessInBack;
            myMaster.headlessInBack.isBehindBlank = false;

            myMaster.headlessInBack = null;
            myMaster.headlessInFront = null;
        }
    }

    public void Exit()
    {
        myMaster.animator.SetBool("evt_train", false);
        myMaster.animator.SetLayerWeight(3, 0f);
    }

    #region Constructor

    public CharacterEventTrain(Character myMaster, CharacterState stateMachine)
    {
        this.myMaster = myMaster;
        this.stateMachine = stateMachine;
    }

    #endregion

    public void IfStateChange()
    {

    }

}
