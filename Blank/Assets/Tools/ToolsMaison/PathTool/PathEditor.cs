using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
/// <summary>
/// Tool to create and edit paths used by PathFollow tool.
/// </summary>
public class PathEditor : EditorWindow
{
    const float WINDOW_SIZE_X = 185;
    const float WINDOW_SIZE_Y = 300;
    public string pathName;
    public bool isLinkedToFirst;
    public bool isEditing;
    public PathObject path;
    public float snapValue = .5f;
    public Mathl.Curve curveToNext;
    [Range(Mathl.MIN_DURATION, Mathl.MAX_DURATION)] public float duration = 1;
    public string syncName;

    [MenuItem("Tools/PathEditor")]
    static void Init()
    {
        Vector2 windowSize = new Vector2(WINDOW_SIZE_X, WINDOW_SIZE_Y);
        EditorWindow window = GetWindow(typeof(PathEditor));
        window.maxSize = windowSize;
        window.minSize = windowSize;
        PathEditor editor = (PathEditor)window;
        editor.Show();
    }

    void OnInspectorUpdate()
    {
        Repaint();
    }

    void OnEnable()
    {
        SceneView.onSceneGUIDelegate += SceneGUI;
    }

    private void OnDisable()
    {
        SceneView.onSceneGUIDelegate -= SceneGUI;
    }

    private void OnGUI()
    {
        InitGUI();
    }

    private void OnHierarchyChange()
    {
        if (path != null)
        {
            PathInEditor p = (PathInEditor)Editor.CreateEditor(path);
            p.path = path;
        }
    }
    #region GUI Init
    void InitGUI()
    {
        GUIPathName();
        GUINewPathButton();
        EditorGUILayout.Separator();
        GUIPathOptions();
        EditorGUILayout.Separator();
        GUILerpCurve();
        EditorGUILayout.Separator();
        GUISyncName();
        GUIPathSyncButton();
    }

    void GUIPathName()
    {
        EditorGUILayout.LabelField("New path name");
        pathName = EditorGUILayout.TextField(pathName);
    }

    void GUISyncName()
    {
        EditorGUILayout.LabelField("New syncronizer name");
        syncName = EditorGUILayout.TextField(syncName);
    }

    void GUINewPathButton()
    {
        if (GUILayout.Button("Create New Path"))
        {
            GameObject o = new GameObject(pathName);
            path = o.AddComponent<PathObject>();
            Selection.activeTransform = path.transform;
            PathInEditor p = (PathInEditor)Editor.CreateEditor(path);
            p.path = path;
        }
    }

    void GUIPathOptions()
    {
        EditorGUILayout.LabelField("Current path : " + (path != null ? path.name : ""));
        isEditing = EditorGUILayout.Toggle("Edit path", isEditing);
        isLinkedToFirst = EditorGUILayout.Toggle("Is linked to first", isLinkedToFirst);
        snapValue = EditorGUILayout.FloatField("Grid snap value", snapValue);
    }

    void GUILerpCurve()
    {
        EditorGUILayout.LabelField("Lerp to next : ");
        curveToNext = (Mathl.Curve)EditorGUILayout.EnumPopup(curveToNext);
        EditorGUILayout.LabelField("Duration");
        duration = EditorGUILayout.Slider(duration, .1f, 100);
    }

    void GUIPathSyncButton()
    {
        if (GUILayout.Button("Create New Syncronizer"))
        {
            GameObject o = new GameObject(syncName + "_PathSync");
            o.AddComponent<PathSync>();
        }
    }
    #endregion

    void SceneGUI(SceneView view)
    {
        if (!Application.isPlaying)
        {
            Event guiEvent = Event.current;
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && path != null)
                OnLeftClic(path, guiEvent);
            UpdateSelection();
        }
    }

    void UpdateSelection()
    {

        if (Selection.activeTransform != null)
        {
            PathObject p = Selection.activeTransform.GetComponent<PathObject>();
            WayPoint wp = Selection.activeTransform.GetComponent<WayPoint>();
            if (!isEditing && (p != null || wp != null))
            {
                if (p == null)
                    path = wp.GetComponentInParent<PathObject>();
                else
                    path = p;
            }
            if (isEditing && path != null && Selection.activeTransform != path.transform)
                Selection.activeTransform = path.transform;
        }
    }

    void OnLeftClic(PathObject p, Event guiEvent)
    {
        if (isLinkedToFirst && p.wayPoints.Count < 1)
            isLinkedToFirst = false;
        if (isEditing)
            AddWayPoint(path, guiEvent);
    }

    void AddWayPoint(PathObject p, Event guiEvent)
    {
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);
        if (Physics.Raycast(mouseRay, out RaycastHit hit))
        {
            Mathl.LerpCurve curve = new Mathl.LerpCurve(curveToNext, duration);
            p.NewWayPoint(hit.point, snapValue, isLinkedToFirst, curve);
        }
        else
        {
            float drawPlaneHeight = 0;
            float distanceToDrawPlane = (drawPlaneHeight - mouseRay.origin.z) / mouseRay.direction.z;
            Vector3 mousePosition = mouseRay.GetPoint(distanceToDrawPlane);
            Mathl.LerpCurve curve = new Mathl.LerpCurve(curveToNext, duration);
            p.NewWayPoint(mousePosition, snapValue, isLinkedToFirst, curve);
        }
    }

}
#endif
