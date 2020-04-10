using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefAvatar : MonoBehaviour
{
    public void SetTrigger(string triggerName)
    {
        GameManager.instance.currentAvatar.animator.SetTrigger(triggerName);
    }

    public void ResetTrigger(string triggerName)
    {
        GameManager.instance.currentAvatar.animator.ResetTrigger(triggerName);
    }

    public void DisableInput()
    {
        GameManager.instance.currentAvatar.DisableInput();
    }

    public void EnableInput()
    {
        GameManager.instance.currentAvatar.EnableInput();
    }

    public void SetMove(float vitesse)
    {
        GameManager.instance.currentAvatar.animator.SetFloat("vitesse", vitesse);
    }

    public void LookAt(Transform lookAtPos)
    {
        Vector3 temp = lookAtPos.position;
        temp.y = GameManager.instance.currentAvatar.tr.position.y;
        GameManager.instance.currentAvatar.tr.LookAt(temp);
    }

    public void EnableEventAnimation()
    {
        GameManager.instance.currentAvatar.animator.SetLayerWeight(3, 1f);
    }

    public void DisableEventAnimation()
    {
        GameManager.instance.currentAvatar.animator.SetLayerWeight(3, 0f);
    }

    public void NextPosInLine()
    {
        //TODO : Applique l'anim de headless de train
        //If isFirstInLine
    }

    public void EnableHead()
    {
        GameManager.instance.currentAvatar.blankHead.enabled = true;
    }

    public void EnterTrain()
    {
        GameManager.instance.currentAvatar.EnterTrain();
    }

    #region ENABLE/DISABLE ABILITY
    public void EnableJump()
    {
        GameManager.instance.spcLvlMan.jumpUnlocked = true;
    }

    public void DisableJump()
    {
        GameManager.instance.spcLvlMan.jumpUnlocked = false;
    }

    public void EnablePerspAdd()
    {
        GameManager.instance.spcLvlMan.perspAddUnlocked = true;
    }

    public void DisablePerspAdd()
    {
        GameManager.instance.spcLvlMan.perspAddUnlocked = false;
    }

    public void EnablePerspSubs()
    {
        GameManager.instance.spcLvlMan.perspSubsUnlocked = true;
    }

    public void DisablePerspSubs()
    {
        GameManager.instance.spcLvlMan.perspSubsUnlocked = false;
    }
    #endregion
}
