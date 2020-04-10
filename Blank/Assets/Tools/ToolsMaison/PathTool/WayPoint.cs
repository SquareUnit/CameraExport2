using UnityEditor;
using UnityEngine;

/// <summary>
/// Point on a path with different options for moving along created path.
/// </summary>
public class WayPoint : MonoBehaviour
{
    public Mathl.LerpCurve lerpToNext;
    public BezierTurn bezierTurn;

    [System.Serializable]
    public class BezierTurn
    {
        const float MIN_START_TIME = .25f, MIN_DURATION = .01f, MAX_DURATION = 10;
        public bool isBezier;
        [Tooltip("When % will the bezier curve start.")]
        [Range(MIN_START_TIME, 1)] public float startTime = .75f;
        [Tooltip("How long does this curve take to complete.")]
        [Range(MIN_DURATION, MAX_DURATION)] public float duration = 1;
        public Mathl.Curve lerpCurve;
        [HideInInspector]
        public float distanceToNext;
    }

    [HideInInspector]
    public WayPoint previous, next;
    [HideInInspector]
    public GameObject path;
    [HideInInspector]
    public Transform tr;
    [HideInInspector]
    public float distanceToNext;

    private void Awake()
    {
        tr = transform;
    }

    public void Init(GameObject path, Vector3 position, Quaternion rotation, Mathl.LerpCurve lerpToNext)
    {
        transform.position = position;
        transform.rotation = rotation;
        this.path = path;
        this.lerpToNext = lerpToNext;
    }
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (IsDrawConnection())
        {
            Gizmos.DrawIcon(transform.position, "WayPoint", true);
            if (next != null)
                DrawConnection(transform.position, next.transform.position);
            if (previous != null)
                DrawConnection(transform.position, previous.transform.position);
            string[] s = name.Split('_');
            if (path.name != s[0])
            {
                s[0] = path.name;
                name = s[0] + "_" + s[1];
            }
           
        }
        else if(path == null)
            path = transform.parent.gameObject;
    }

    bool IsDrawConnection()
    {
        return path != null
            && (Selection.activeTransform == path.transform
            || Selection.activeTransform == transform);
    }

    void DrawConnection(Vector3 start, Vector3 end)
    {
        Handles.color = Color.red;
        Handles.DrawLine(start, end);
        if (Selection.activeTransform != path.transform)
            Gizmos.DrawIcon(end, "WayPoint", true);
    }
#endif
}
