using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NmeStateSearch : NmeStateMachine
{

    public override void Idle()
    {
        myMaster.ChangeState(NmeController.NmeStates.IDLE);
    }
    public override void Chase()
    {
        myMaster.ChangeState(NmeController.NmeStates.CHASE);
    }
    public override void Search()
    {
        myMaster.ChangeState(NmeController.NmeStates.SEARCH);
    }
    public override void Defend()
    {
        myMaster.ChangeState(NmeController.NmeStates.DEFEND);
    }



    public override void AFaireChaqueFrame()
    {
        throw new System.NotImplementedException();
    }

    public NmeStateSearch(NmeController myMaster) : base(myMaster) { }


}
