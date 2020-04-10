using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadlessIdle : IStates
{
    private HeadlessNPC user; // =master

    #region CONSTRUCTOR
    /// <summary> Fire the constructor of a state. In this state, the drone stays immobile, can detect & look around at interval if desired </summary>
    /// <param name="user"> The drone instance using the state machine</param>
    public HeadlessIdle(HeadlessNPC user)
    {
        this.user = user;
    }
    #endregion

    public void Enter() 
    {
        // What happens before you enter the update loop of the state
        // Think sound, FX, animations, coroutines

        // Anim Placeholder
        /*user.animator.SetBool("idleVariationHat", true);
        user.animator.SetBool("idleVariationTie", true);*/

        //user.animator.SetTrigger("idle"); // anim pas bonne
        user.animator.SetBool("idleVariationAll", true);
        user.animator.SetLayerWeight(1, 1f);

        if (user.isSitting || user.isSittingVar) { user.Invoke("PlayRandomAnim", Random.Range(user.randomTimeMin, user.randomTimeMax)); }
    }

    public void IfStateChange()
    {

    }

    public void StateUpdate()
    {
        // Run as an update
        //user.animator.SetTrigger("idle"); // anim pas bonne
    }

    public void Exit() 
    {
        // What happens before you change state
        // Think sound, FX, animations ans stopping coroutines

        // Anim Placeholder
        /*user.animator.SetBool("idleVariationHat", false);
        user.animator.SetBool("idleVariationTie", false);

        user.animator.ResetTrigger("idle");*/

        user.animator.SetBool("idleVariationAll", false);
        user.animator.SetLayerWeight(1, 0f);
        user.CancelInvoke("PlayRandomAnim");
    }

}
