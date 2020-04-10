using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.ProGrids;

/// <summary>
/// Path created by PathEditor tool to be used by PathFollow tool.
/// </summary>
public class PathObject : MonoBehaviour
{
    const int BEZIER_SYNC_POINTS = 1000;
    const string WAYPOINT_NAME = "_WayPoint";
    [Tooltip("Will target change direction at the end of path?")]
    public bool isLooping = true;
    [Tooltip("Checking this box will sync all waypoints to the total duration selected below")]
    public bool useTotalDuration;
    [Tooltip("Total duration of the path")]
    [Range(Mathl.MIN_DURATION, Mathl.MAX_DURATION)]public float totalDuration = 1;

    [HideInInspector]
    public WayPoint currentWp, current, next, startPoint;
    [HideInInspector]
    public List<WayPoint> wayPoints = new List<WayPoint>();

    private void Start()
    {
        if (useTotalDuration)
            SyncAtDuration();
    }

    /// <summary>
    /// Returns total distance between connected points.
    /// </summary>
    /// <returns></returns>
    public float GetTotalDistance()
    {
        float dist = 0;
        for (int i = 0; i < wayPoints.Count; i++)
            dist += DistanceToNext(wayPoints[i], wayPoints[i].next);
        return dist;
    }

    /// <summary>
    /// Set duration of all waypoints compared with a total duration and distance.
    /// </summary>
    public void SetLocalDuration(float totalDist, float totalDuration)
    {
        float localDuration = 0;
        float bezierDuration = 0;
        for (int i = 0; i < wayPoints.Count; i++)
        {
            localDuration = (wayPoints[i].distanceToNext / totalDist) * totalDuration;
            if (wayPoints[i].bezierTurn.isBezier)
            {
                bezierDuration = (wayPoints[i].bezierTurn.distanceToNext / totalDist) * totalDuration;
                wayPoints[i].bezierTurn.duration = bezierDuration;
            }
            wayPoints[i].lerpToNext.duration = localDuration;
        }
    }

    /// <summary>
    /// Sync all waypoints to path total duration.
    /// </summary>
    public void SyncAtDuration()
    {
        wayPoints.RemoveAll(WayPoint => WayPoint == null);
        bool isCycle = IsCycle(wayPoints[0], wayPoints[0]);
        float count = (isCycle ? wayPoints.Count : wayPoints.Count - 1);
        foreach (WayPoint wp in wayPoints)
            count = (wp.bezierTurn.isBezier ? count + 1 : count);
        float d = totalDuration / count;
        foreach (WayPoint wp in wayPoints)
        {
            if (wp.bezierTurn.isBezier)
            {
                wp.bezierTurn.duration = d;
                wp.lerpToNext.duration = d;
            }
            else
                wp.lerpToNext.duration = d;
        }
    }

    /// <summary>
    /// Adds a new waypoint to this path.
    /// </summary>
    public void NewWayPoint(Vector3 position, float snapValue, bool isLinkedToFirst, Mathl.LerpCurve curveToNext)
    {
        Vector3 snapValues = new Vector3(snapValue, snapValue, snapValue);
        //Progrid dependency
        //position = pg_Util.SnapValue(position, snapValue);

        position = GridSnap(position, snapValues);
        GameObject op = new GameObject(name + WAYPOINT_NAME + (wayPoints.Count + 1).ToString());
        WayPoint wp = op.AddComponent<WayPoint>();
        op.transform.SetParent(transform);
        if (currentWp != null)
            InitCurrentWayPoint(wp, isLinkedToFirst);
        else
            InitFirstWayPoint(wp);
        wp.Init(gameObject, position, transform.rotation, curveToNext);
        wayPoints.Add(wp);
    }
     
    Vector3 GridSnap(Vector3 position, Vector3 snapValues)
    {
        float x = Mathf.Round(position.x / snapValues.x) * snapValues.x;
        float y = Mathf.Round(position.y / snapValues.y) * snapValues.y;
        float z = Mathf.Round(position.z / snapValues.z) * snapValues.z;
        return new Vector3(x, y, z);
    }

    void InitCurrentWayPoint(WayPoint wp, bool isLinkedToFirst)
    {
        if (isLinkedToFirst)
        {
            wp.next = wayPoints[0];
            wayPoints[0].previous = wp;
        }
        else
            wayPoints[0].previous = null;
        currentWp.next = wp;
        wp.previous = currentWp;
        currentWp = wp;
    }
    
    bool IsCycle(WayPoint start, WayPoint current)
    {
        if (current.next)
        {
            if (start == current.next)
                return true;
            else
                return IsCycle(start, current.next);
        }
        else
            return false;
    }

    void InitFirstWayPoint(WayPoint wp)
    {
        currentWp = wp;
        current = currentWp;
        startPoint = wp;
    }

    float DistanceToNext(WayPoint current, WayPoint next)
    {
        Vector3 currentPos = current.transform.position;
        if (next)
        {
            Vector3 nextPos = next.transform.position;
            Vector3 lastBezier = Vector3.Lerp(currentPos, nextPos, (current.bezierTurn.isBezier ? (1 - current.bezierTurn.startTime) : 0));
            Vector3 bezierStart = Vector3.Lerp(currentPos, nextPos, (next.bezierTurn.isBezier ? next.bezierTurn.startTime : 1));

            current.distanceToNext = Vector3.Distance(lastBezier, bezierStart);

            if (next.bezierTurn.isBezier && next.next != null)
                return current.distanceToNext + BezierToNext(next, bezierStart, nextPos);
            return current.distanceToNext;
        }
        else if (current.previous)
            current.distanceToNext = Vector3.Distance(currentPos, current.previous.transform.position);
        return 0;
    }

    float BezierToNext(WayPoint next, Vector3 bezierStart, Vector3 nextPos)
    {
        Vector3 bezierEnd = Vector3.Lerp(nextPos, next.next.transform.position, (1 - next.bezierTurn.startTime));
        next.bezierTurn.distanceToNext = BezierDist(bezierStart, nextPos, bezierEnd);
        return next.bezierTurn.distanceToNext;
    }

    float BezierDist(Vector3 bezierStart, Vector3 nextPos, Vector3 bezierEnd)
    {
        float j = 1f / BEZIER_SYNC_POINTS;
        Vector3 point;
        Vector3 p = bezierStart;
        float d = 0;
        for (int i = 1; i <= BEZIER_SYNC_POINTS - 1; i++)
        {
            point = Mathl.QuadraticBezierPoint(bezierStart, nextPos, bezierEnd, j * i);
            d += Vector3.Distance(p, point);
            p = point;
        }
        d += Vector3.Distance(p, bezierEnd);
        return d;
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(PathObject))]
[CanEditMultipleObjects]
public class PathInEditor : Editor
{
    public PathObject path;
    private void OnEnable()
    {
        EditorApplication.hierarchyChanged += OnHierarchyChange;
    }

    private void OnDisable()
    {
        EditorApplication.hierarchyChanged -= OnHierarchyChange;
    }
         
    private void OnHierarchyChange()
    {
        if (path != null && !Application.isPlaying)
            FilterCurrentPath();
    }

    void FilterCurrentPath()
    {
        if (path != null && path.wayPoints.Count > 0)
        {
            path.wayPoints.RemoveAll(Object => Object == null);
            for (int i = 0; i < path.wayPoints.Count; i++)
            {
                if (IsNotFirst(i) && HasPreviousChange(path.wayPoints[i], path.wayPoints[i - 1]))
                    path.wayPoints[i].previous = path.wayPoints[i - 1];

                if (IsNotLast(i) && HasNextChange(path.wayPoints[i], path.wayPoints[i + 1]))
                    path.wayPoints[i].next = path.wayPoints[i + 1];
            }
        }
    }

    bool IsNotFirst(int i)
    {
        return i != 0 && i - 1 > 0;
    }

    bool IsNotLast(int i)
    {
        return i != path.wayPoints.Count - 1 && path.wayPoints.Count > i + 1;
    }

    bool HasPreviousChange(WayPoint current, WayPoint previous)
    {
        return current.previous != previous;
    }

    bool HasNextChange(WayPoint current, WayPoint next)
    {
        return current.next != next;
    }
      
}
#endif
