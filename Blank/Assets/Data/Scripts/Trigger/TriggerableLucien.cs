using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerableLucien : Triggerable
{
    public Renderer render;
    public Collider coli;

    void Start()
    {
        render = GetComponentInChildren<Renderer>();
        coli = GetComponentInChildren<Collider>();

    }

    public override void Activate()
    {
       
            if (render != null)
                render.enabled = !render.enabled;

            if (coli != null)
                coli.enabled = !coli.enabled;
       
    }

    public override void Deactivate()
    {
       
            if (render != null)
                render.enabled = !render.enabled;

            if (coli != null)
                coli.enabled = !coli.enabled;
   
    }

    public override void Toggle()
    {
        startActivated = !startActivated;
        if (startActivated) Activate();
        else Deactivate();
    }




}