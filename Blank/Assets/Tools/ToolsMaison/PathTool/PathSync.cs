using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class PathSync : MonoBehaviour
{
    public enum Speed { UltraFast, SuperFast, VeryFast, Fast, Medium, Slow, VerySlow, SuperSlow, UltraSlow }
    [SerializeField] Speed speed = (Speed)4;
    [SerializeField] [Range(1, 100)] float weight;
    [SerializeField] bool isSyncing;
    public PathObject pathToSyncronise;

    void Awake()
    {
        isSyncing = true;
        if (isSyncing)
        {
            float leaderDist = pathToSyncronise.GetTotalDistance();
            float totalDuration = (((float)speed + 1) * weight) + .1f;

            pathToSyncronise.SetLocalDuration(leaderDist, totalDuration);
            Transform tr = transform;
            foreach (Transform child in tr)
            {
                PathObject p = child.GetComponent<PathObject>();
                if (p)
                {
                    if (p != pathToSyncronise)
                    {
                        p.GetTotalDistance();
                        p.SetLocalDuration(leaderDist, totalDuration);
                    }

                }
            }
        }
    }
}

