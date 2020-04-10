/*Script cree le : 11-04-2019
 * Par: Steven Judge & Félix Desrosiers
 * Derniere modification:22-04-2019
 * Par:Steven Judge & Félix Desrosiers
 * Description: Trigger qui vérifie par quel cote il se fait activer (Devant-derière)
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalTriggerable : Triggerable
{
    private bool parDevant = false;
    private bool parDerriere = false;
    public Triggerable actionParDevantZ;
    public Triggerable actionParDerriereZ;

    private void Update()
    {
        if (myTriggers.Count > 0)
        {
            if (myTriggers[0].dot > 0)
            {
                parDerriere = true;
            }
            else if (myTriggers[0].dot < 0)
            {
                parDevant = true;
            }
        }
    }

    public override void Activate()
    {
        if (parDerriere == true)
        {
            actionParDerriereZ.Activate();
        }
        else if (parDevant == true)
        {
            actionParDevantZ.Activate();
        }
    }

    public override void Deactivate()
    {
        if (parDerriere == true)
        {
            actionParDerriereZ.Deactivate();
        }
        else if (parDevant == true)
        {
            actionParDevantZ.Deactivate();
        }
    }

    public override void Toggle()
    {
        startActivated = !startActivated;
        if (startActivated) Activate();
        else Deactivate();
    }
}