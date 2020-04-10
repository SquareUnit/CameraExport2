using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NMEFollow : PathFollow
{
    [HideInInspector]
    public bool isFollowing = false;

    private void Start()
    {
        lerpable = GetComponent<Lerpable>();
        lerpable.active = true;
        if (path != null)
        {
            if (startPoint > path.wayPoints.Count)
                startPoint = path.wayPoints.Count;
            if (path.wayPoints[startPoint - 1])
                Init();
        }
        active = true;
        lerpable.active = true;
        Activate.AddListener(() => SetActive(true));
        Deactivate.AddListener(() => SetActive(false));
        GameObject o = new GameObject("Target");
        target = o.transform;
        target.position = transform.position;
    }


    protected override void UpdateLerp()
    {
        if (isFollowing)
        {
            lerpable.Lerp();
            UpdateTarget();
            if (path != null && lerpable != null && target && active)
                Lerp();
        }
    }
}
