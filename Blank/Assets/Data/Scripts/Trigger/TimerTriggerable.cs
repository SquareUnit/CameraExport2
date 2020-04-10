/*Script cree le : 22-04-2019
 * Par: Steven Judge
 * Derniere modification:22-04-2019
 * Par:Steven Judge
 * Description: Timer qui desactive le trigger X nb de seconde apres son activation
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerTriggerable : Triggerable
{
    public Triggerable triggerable;
    [Range(2f, 60f)]
    public float timer = 5f;

    public override void Activate()
    {
        triggerable.Activate();
        triggerable.Invoke("Deactivate", timer);
    }

    public override void Deactivate()
    {
        triggerable.Deactivate();
        triggerable.Invoke("Activate", timer);
    }

    public override void Toggle()
    {
        startActivated = !startActivated;
        if (startActivated) Activate();
        else Deactivate();
    }

}
