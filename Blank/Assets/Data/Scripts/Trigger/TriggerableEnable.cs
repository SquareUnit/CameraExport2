/*Script cree le : 11-04-2019
 * Par: Steven Judge 
 * Derniere modification:22-04-2019
 * Par:Steven Judge 
 * Description: trigger pour faire disparaitre ou apparaitre un Renderer et un Collider
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerableEnable : Triggerable
{
    public Renderer render;
    public Collider[] coli;

    void Start()
    {
        render = GetComponentInChildren<Renderer>();
        coli = GetComponentsInChildren<Collider>();
    }

    public override void Activate()
    {
        if (state != 1)
        {
            if(render != null)
            render.enabled = !render.enabled;

            if(coli.Length > 0)
            {
                foreach(Collider i in coli)
                {
                    i.enabled = !i.enabled;
                }
            }
            
        }
        state = 1;
    }

    public override void Deactivate()
    {
        if (state != 2)
        {
            if (render != null)
                render.enabled = !render.enabled;

            if (coli.Length > 0)
            {
                foreach (Collider i in coli)
                {
                    i.enabled = !i.enabled;
                }
            }

        }
        state = 2;
    }

    public override void Toggle()
    {
        startActivated = !startActivated;
        if (startActivated) Activate();
        else Deactivate();
    }
}