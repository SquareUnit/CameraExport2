using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class ScriptedEvent : Triggerable
{
    public List<UnityEvent> @event;
    public bool dontDecrouchBlank;
    

    public override void Activate()
    {
        if(GameManager.instance.currentAvatar != null && GameManager.instance.currentAvatar.stateMachine.currentState == GameManager.instance.currentAvatar.stateMachine.turn)
        {
            Invoke("DelayActivate", 0.8f);
        }
        else if (dontDecrouchBlank && GameManager.instance.currentAvatar.isCrouching)
        {
            Debug.Log("CrouchingTigerHiddenDragon");
            GameManager.instance.currentAvatar.stateMachine.ChangeState(GameManager.instance.currentAvatar.stateMachine.idle);
            @event.ForEach(n => n.Invoke());
        }
        else
        {
            @event.ForEach(n => n.Invoke());
        }
    }

    public override void Deactivate()
    {
    }

    public override void Toggle()
    {
    }

    public void PlayEvent()
    {
        Activate();        
    }

    public void DelayActivate()
    {
        @event.ForEach(n => n.Invoke());
    }

}
