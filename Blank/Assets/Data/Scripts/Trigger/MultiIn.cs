/*Script cree le : 05-04-2019
 * Par: Steven Judge
 * Derniere modification:22-04-2019
 * Par:Steven Judge
 * Description: Plusieur trigger pour un seul objet aActiver -> 2+ Triggers pour 1 Triggerable
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiIn : Triggerable
{
    public Triggerable triggerable;
    [Range(2, 5)]
    public int nbTrigger = 2;
    [HideInInspector] private int compteur = 0;

    public override void Activate()
    {
        foreach (Trigger t in myTriggers)
        {
            if (!t.isUse)
                return;
        }
        Enable();
    }

    public override void Deactivate()
    {
        foreach (Trigger t in myTriggers)
        {
            if (t.isUse)
                return;
        }
        Disable();
    }

    public override void Toggle()
    {

    }

    private void ResetCompteur()
    {
        compteur = 0;
    }

    private void Enable()
    {
        triggerable.Activate();
        ResetCompteur();
    }

    private void Disable()
    {
        triggerable.Deactivate();
        ResetCompteur();
    }
}