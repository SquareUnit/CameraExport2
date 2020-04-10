using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeFollow : MonoBehaviour
{
    [Tooltip("Path to follow")]
    public PathObject path;
    [Tooltip("Starting waypoint")]
    [SerializeField] [Range(1, PathFollow.MAX_WAYPOINTS)] int startPoint = 1;
    public bool looksAtDirection;
    public bool startActive = true;
    [SerializeField] bool isActive;
    int current;

    void Awake()
    {
        if (isActive)
        {
            Transform tr = transform;
            current = startPoint;
            foreach (Transform child in tr)
            {
                PathFollow p = child.GetComponent<PathFollow>();
                p.path = path;
                p.looksAtDirection = looksAtDirection;
                p.startActive = startActive;
                p.startPoint = current;
                current++;
            }
        }
    }
}
