using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NmeStateMachine
{
    protected NmeController myMaster;


    public abstract void Idle();
    public abstract void Chase();
    public abstract void Search();
    public abstract void Defend();


    public abstract void AFaireChaqueFrame();


    public NmeStateMachine(NmeController myMaster)
    {
        this.myMaster = myMaster;
    }
}
