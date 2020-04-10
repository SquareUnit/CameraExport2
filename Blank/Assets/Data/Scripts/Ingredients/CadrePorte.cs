using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CadrePorte : Triggerable
{

    public HeadHunterNME headHunter;
    public Trigger myTrig;
    public Trigger myTrig2;
    public Trigger myTrigPlayer;
    public GameObject hHDestination;
    public GameObject hHDestination2;
    public GameObject hHLookAt;
    public bool canPlayAnim;
    private float closestPointToHH;
    public bool headHunterInTrigger = false;

    public override void Activate()
    {
        if (myTrig.activant != null && myTrig.activant.CompareTag("HeadHunter") || myTrig2.activant != null && myTrig2.activant.CompareTag("HeadHunter"))
        {
            headHunterInTrigger = true;

            if (myTrig.activant != null && myTrig.activant.CompareTag("HeadHunter"))
            {
                headHunter = myTrig.activant.GetComponent<HeadHunterNME>();
            }
            else
            {
                headHunter = myTrig2.activant.GetComponent<HeadHunterNME>();
            }
        }



        if (myTrigPlayer.activant != null && myTrigPlayer.activant.CompareTag("Player") && headHunter != null)
        {
            if (headHunter.stateMachine.currentState == headHunter.patrol || headHunter.stateMachine.currentState == headHunter.turn)
            {
                canPlayAnim = false;
            }
            else
            {
                canPlayAnim = true;
            }
        }
    }

    public override void Deactivate()
    {
    }

    private void Update()
    {
        if (headHunterInTrigger && canPlayAnim)
        {
            closestPointToHH = Vector3.Distance(headHunter.tr.position, hHDestination.transform.position);

            if (closestPointToHH < Vector3.Distance(headHunter.tr.position, hHDestination2.transform.position))
            {
                headHunter.door.headHunterDestination = hHDestination.transform.position;
            }
            else
            {
                headHunter.door.headHunterDestination = hHDestination2.transform.position;
            }
            headHunter.door.headHunterLookAt = hHLookAt.transform.position;
            headHunter.door.placeHH = true;
            headHunter.stateMachine.ChangeState(headHunter.door);
            canPlayAnim = false;
        }



        if (headHunter != null)
        {
            if (headHunter.stateMachine.currentState == headHunter.patrol || headHunter.stateMachine.currentState == headHunter.turn)
            {
                canPlayAnim = false;
                headHunterInTrigger = false;
            }
        }

    }

    public override void Toggle()
    {

    }
}
