/*Script cree le : 05-04-2019
 * Derniere modification:22-04-2019
 * Par:Steven Judge
 * Description: Permet d'avoir un delais entre le temps d'activation du trigger et l'activation de l'objet
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delay : Triggerable
{
    public Triggerable triggerable;
    [Range(0.05f, 30f)]
    public float delay = 5f;

    public override void Activate()
    {
        if (state != 1)
        {
            Enable();
            state = 1;
        }
    }

    public override void Deactivate()
    {
        if (state != 2)
        {
            Disable();
            state = 2;
        }
    }

    public override void Toggle()
    {
        startActivated = !startActivated;
        if (startActivated) Invoke("On", delay);
        else Invoke("Off", delay);
    }

    private void On()
    {
        triggerable.Activate();
    }

    private void Off()
    {
        triggerable.Deactivate();
    }

    private void Enable()
    {
        Invoke("On", delay);
    }

    private void Disable()
    {
        Invoke("Off", delay);
    }
}