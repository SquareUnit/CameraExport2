using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerableRotation : Triggerable
{
    Transform tr;
    public float quartDeTour = 1f;
    public float duration = 1f;
    public enum TriggerRotation { AxeY, AxeX, AxeZ }
    public TriggerRotation orientation;
    float startTime;
    Quaternion start;
    Quaternion end;
    bool active;
    [HideInInspector] public Quaternion rotatation;
    bool firstTime = true, deactivate, activate;

    public void Awake()
    {
        tr = transform;
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
            if (orientation == TriggerRotation.AxeY)
            {
                rotatation = Quaternion.Euler(0, 90 * quartDeTour, 0);
                end = tr.rotation * rotatation;
            }
            else if (orientation == TriggerRotation.AxeX)
            {
                rotatation = Quaternion.Euler(90 * quartDeTour, 0, 0);
                end = tr.rotation * rotatation;
            }
            else if (orientation == TriggerRotation.AxeZ)
            {
                rotatation = Quaternion.Euler(0, 0, 90 * quartDeTour);
                end = tr.rotation * rotatation;
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
            if (orientation == TriggerRotation.AxeY)
            {
                rotatation = Quaternion.Euler(0, -90 * quartDeTour, 0);
                end = tr.rotation * rotatation;
            }
            else if (orientation == TriggerRotation.AxeX)
            {
                rotatation = Quaternion.Euler(-90 * quartDeTour, 0, 0);
                end = tr.rotation * rotatation;
            }
            else if (orientation == TriggerRotation.AxeZ)
            {
                rotatation = Quaternion.Euler(0, 0, -90 * quartDeTour);
                end = tr.rotation * rotatation;
            }
            StartLerp();
        }
        state = 2;
        firstTime = false;
        deactivate = false;
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
        tr.rotation = Quaternion.Lerp(tr.rotation, end, t);
        if (t >= 1)
            active = false;
    }

    void InvertPosition()
    {
        Quaternion temp = start;
        start = end;
        end = start;
    }
}