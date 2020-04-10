/*Script cree le : 05-04-2019
 * Par: Steven Judge
 * Derniere modification:22-04-2019
 * Par:Steven Judge
 * Description:Permet au Trigger de "trigger" plus d'un objet aActiver -> 1 trigger pour 2+ Triggerables
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiOut : Triggerable
{
    public Triggerable[] triggerable;
    [HideInInspector]
    public int stateMOD;

    public override void Activate()
    {
        if (stateMOD != 1)
        {
            Enable();
            stateMOD = 1;
        }
    }

    public override void Deactivate()
    {
        if (stateMOD != 2)
        {
            Disable();
            stateMOD = 2;
        }
    }

    public override void Toggle()
    {
        startActivated = !startActivated;
        if (startActivated) Enable();
        else Disable();
    }

    private void Enable()
    {
        for (int i = 0; i < triggerable.Length; i++)
        {
            triggerable[i].Activate();
        }
    }

    private void Disable()
    {
        for (int i = 0; i < triggerable.Length; i++)
        {
            triggerable[i].Deactivate();
        }
    }
}