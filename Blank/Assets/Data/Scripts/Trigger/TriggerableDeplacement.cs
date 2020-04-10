/*Script cree le : 11-04-2019
 * Par: Steven Judge 
 * Derniere modification:22-04-2019
 * Par:Steven Judge
 * Description: Trigger pour déplacer un objet sur son axe Horizontal, Vertical ou en Profondeur
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerableDeplacement : Triggerable
{
    Transform tr;
    public float distance = 2f;
    public float duration = 1f;
    public enum TriggerDeplacement { Horizontal, Verticale, Profondeur }
    public TriggerDeplacement orientation;
    private Vector3 velocite = Vector3.zero;
    Vector3 start;
    Vector3 end;
    float startTime;
    bool active, firstTime = true, deactivate, activate;
    
    public void Awake()
    {
        tr = transform;
        start = tr.position;
    }

    public void Update()
    {
        if (active)
            Lerp();
        else if (activate)
            ActivateUpdate();
        else if (deactivate)
            DeactivateUpdate();
    }

    public override void Activate()
    {
        activate = true;
    }

    public override void Deactivate()
    {
        deactivate = true;
    }

    void ActivateUpdate()
    {
        if (!firstTime)
            InvertPosition();
        if (state != 1)
        {
            if (orientation == TriggerDeplacement.Verticale)
            {
                Vector3 ouvre = new Vector3(0f, distance, 0f);
                end = tr.position + ouvre;
            }
            else if (orientation == TriggerDeplacement.Horizontal)
            {
                Vector3 ouvre = new Vector3(distance, 0f, 0f);
                end = tr.position + ouvre;
            }
            else if (orientation == TriggerDeplacement.Profondeur)
            {
                Vector3 ouvre = new Vector3(0f, 0f, distance);
                end = tr.position + ouvre;
            }
            StartLerp();
        }
        state = 1;
        firstTime = false;
        activate = false;
    }

    void DeactivateUpdate()
    {
        if (!firstTime)
            InvertPosition();
        if (state != 2)
        {
            if (orientation == TriggerDeplacement.Verticale)
            {
                Vector3 ferme = new Vector3(0f, distance, 0f);
                end = tr.position - ferme;
            }
            else if (orientation == TriggerDeplacement.Horizontal)
            {
                Vector3 ferme = new Vector3(distance, 0f, 0f);
                end = tr.position - ferme;
            }
            else if (orientation == TriggerDeplacement.Profondeur)
            {
                Vector3 ferme = new Vector3(0f, 0f, distance);
                end = tr.position - ferme;
            }
            StartLerp();
        }
        state = 2;
        deactivate = false;
    }

    void InvertPosition()
    {
        Vector3 temp = start;
        start = end;
        end = start;
    }

    public override void Toggle()
    {
        startActivated = !startActivated;
        if (startActivated) Activate();
        else Deactivate();
    }

    void StartLerp()
    {
        startTime = Time.time;
        active = true;
    }

    void Lerp()
    {
        float t = Mathf.SmoothStep(0, 1, (Time.time - startTime) / duration);
        transform.position = Vector3.Lerp(start, end, t);
        if (t >= 1)
        {
            active = false;
            start = transform.position;  
        }
    }

    public void ResetState()
    {
        if(state != 0)
        {
            state = 0;
        }
    }
}