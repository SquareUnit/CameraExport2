using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Add this to gameObject to make target follow a path created with PathEditor tool.
/// </summary>
[RequireComponent(typeof(Lerpable))]
public class PathFollow : MonoBehaviour
{
    public const int MAX_WAYPOINTS = 300;

    [Tooltip("Path to follow")]
    public PathObject path;
    [Tooltip("Starting waypoint")]
    [Range(1, MAX_WAYPOINTS)] public int startPoint = 1;
    [Tooltip("Target that follows the path")]
    public Transform target;
    [Tooltip("Target is following path on application start")]
    public bool startActive = true;
    [Tooltip("Target rotates towards the direction of the next frame")]
    public bool looksAtDirection;
    [HideInInspector]
    public bool autoNext = true;

    [HideInInspector]
    public UnityEvent Activate = new UnityEvent();
    [HideInInspector]
    public UnityEvent Deactivate = new UnityEvent();
    [HideInInspector]
    public bool isBezierLerp;
    [HideInInspector]
    public WayPoint current, next;
    [HideInInspector]
    public float angle;

    bool forward;
    protected Lerpable lerpable;
    Vector3 start, end;
    Vector3 bezierStartPoint, bezierEndPoint;
    Vector3 targetDirection, nextPos;
    GameObject currentTarget;
    protected bool active;

    private void Start()
    {
        lerpable = GetComponent<Lerpable>();
        lerpable.active = true;
        if (target)
            SetTarget();
        else if (transform.GetComponent<PathObject>() == null)
            target = transform;
        if (path != null)
        {
            if (startPoint > path.wayPoints.Count)
                startPoint = path.wayPoints.Count;
            if (path.wayPoints[startPoint - 1])
                Init();
        }
        active = startActive;
        Activate.AddListener(() => SetActive(true));
        Deactivate.AddListener(() => SetActive(false));
    }

    public WayPoint GetStartPoint()
    {
        return path.wayPoints[startPoint - 1];
    }

    void Update()
    {
        UpdateLerp();
    }


    protected void UpdateTarget()
    {
        if (target && currentTarget != target.gameObject)
            SetTarget();
    }

    /// <summary>
    /// Activate or deactivate follow.
    /// </summary>
    public void SetActive(bool isActive)
    {
        active = isActive;
    }

    /// <summary>
    /// Is follow active?
    /// </summary>
    public bool IsActive()
    {
        return active;
    }
    /// <summary>
    /// Direction of target following path for this frame. 
    /// </summary>
    public Vector3 GetTargetDirection()
    {
        return targetDirection;
    }

    public float GetBezierAngle(Transform target)
    {
        Vector3 bEndForward = next.tr.position - bezierEndPoint;
        bEndForward = bEndForward.normalized;
        return Vector3.Angle(target.position, bEndForward);
    }
    /// <summary>
    /// True if target following path is moving for this frame.
    /// </summary>
    public bool IsTargetMoving()
    {
        return lerpable.IsLerping();
    }

    /// <summary>
    /// True if target following path is turning for this frame.
    /// </summary>
    public bool IsTargetTurning()
    {
        return isBezierLerp;
    }

    protected virtual void UpdateLerp()
    {
        if (active != lerpable.active)
            lerpable.active = active;
        lerpable.Lerp();
        UpdateTarget();
        if (path != null && lerpable != null && target && active)
            Lerp();
    }

    protected void Lerp()
    {
        if (isBezierLerp)
            LerpBezier();
        else
            LerpDefault();
    }

    #region Target & Direction
    protected void Init()
    {
        current = path.wayPoints[startPoint - 1];
        if (target && path)
            target.transform.position = path.wayPoints[startPoint - 1].transform.position;
        InitDirection();
        InitDefaultLerp();
    }

    protected void SetTarget()
    {
        bool targetIsPath = target.GetComponent<PathObject>() != null;
        if (!targetIsPath)
            currentTarget = target.gameObject;
        else
            ClearTarget();
    }

    void ClearTarget()
    {
        target = null;
        currentTarget = null;
    }

    void InitDirection()
    {
        if (current.next == null)
        {
            if (current.previous)
                next = current.previous;
            forward = false;
        }
        else
        {
            next = current.next;
            forward = true;
        }
    }

    void UpdateTargetPosition(Vector3 nextPosition)
    {
        if (looksAtDirection)
            target.transform.LookAt(nextPosition);
        nextPos = nextPosition;
        targetDirection = nextPosition - target.transform.position;
        target.transform.position += targetDirection;

    }

    public void GoToNext()
    {
        current = next;
        if (forward)
            GoForward();
        else
            GoBackward();
    }

    void GoForward()
    {
        if (current.next == null && path.isLooping)
        {
            forward = false;
            next = current.previous;
        }
        else if (current.next != null)
            next = current.next;
    }

    void GoBackward()
    {
        if (current.previous == null && path.isLooping)
        {
            forward = true;
            next = current.next;
        }
        else if (current.previous != null)
            next = current.previous;
    }
    #endregion

    #region Bezier lerp
    void InitBezierLerp()
    {
        lerpable.StopLerp();
        isBezierLerp = true;
        Mathl.LerpCurve lc = new Mathl.LerpCurve(next.bezierTurn.lerpCurve, next.bezierTurn.duration);
        lerpable.Init(lc);
        lerpable.StartLerp();
        InitBezierPositions();
    }

    void InitBezierPositions()
    {
        bezierStartPoint = Vector3.Lerp(current.tr.position, next.tr.position, next.bezierTurn.startTime);
        if (forward && next.next)
            bezierEndPoint = Vector3.Lerp(next.tr.position, next.next.tr.position, (1 - next.bezierTurn.startTime));
        else if (next.previous)
            bezierEndPoint = Vector3.Lerp(next.tr.position, next.previous.tr.position, (1 - next.bezierTurn.startTime));
        else
            bezierEndPoint = next.tr.position;
        angle = GetBezierAngle(target);
    }

    void LerpBezier()
    {
        if (!lerpable.IsLerping())
            EndBezierLerp();
        else if (target)
            UpdateTargetPosition(lerpable.Lerp(bezierStartPoint, next.tr.position, bezierEndPoint));
    }

    void EndBezierLerp()
    {
        if (autoNext)
        {
            NextWayPoint();
            isBezierLerp = false;
        }
    }
    #endregion

    #region Default lerp
    void InitDefaultLerp()
    {
        lerpable.Init(current.lerpToNext);
        lerpable.StartLerp();
        start = (current.bezierTurn.isBezier ? Vector3.Lerp(current.tr.position, next.tr.position, (1 - current.bezierTurn.startTime)) : current.tr.position);
        end = (next.bezierTurn.isBezier ? Vector3.Lerp(current.tr.position, next.tr.position, next.bezierTurn.startTime) : next.tr.position);
    }

    void LerpDefault()
    {
        if (!lerpable.IsLerping() && !next.bezierTurn.isBezier)
            EndDefaultLerp();
        else if (!lerpable.IsLerping() && next != null && next.bezierTurn.isBezier)
            InitBezierLerp();
        else if (target)
            UpdateTargetPosition(lerpable.Lerp(start, end));
    }

    void EndDefaultLerp()
    {
        if (autoNext)
            NextWayPoint();
    }
    #endregion

    public void NextWayPoint()
    {
        GoToNext();
        InitDefaultLerp();
    }
}


