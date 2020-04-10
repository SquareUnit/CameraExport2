using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneFovEditor :MonoBehaviour
{
    // DEPRECATED : Je vais copier le code et l'enlever plus tard avec Dave

    //using UnityEditor;
    //[CustomEditor (typeof(DroneNME))]
    //public class DroneFovEditor : Editor

    //private Transform tr;
    //private float dist;
    //float x, y;

    //private void OnSceneGUI()
    //{
    //    DroneNME user = (DroneNME)target;
    //    tr = user.tr;
    //    Handles.color = Color.white;
    //    Handles.DrawWireArc(user.transform.position, Vector3.up, Vector3.forward, 360.0f, user.fovRadius);
    //    Vector3 viewAngleA = user.ThetafromForward(-user.viewAngle / 2, false);
    //    Vector3 viewAngleB = user.ThetafromForward(user.viewAngle / 2, false);

    //    Handles.DrawLine(user.transform.position, user.transform.position + viewAngleA * user.fovRadius);
    //    Handles.DrawLine(user.transform.position, user.transform.position + viewAngleB * user.fovRadius);

    //    dist = Vector3.Distance(user.transform.position, user.transform.position + Vector3.forward * user.fovRadius);
    //    x = dist * Mathf.Cos((user.viewAngle / 2) * Mathf.Deg2Rad);
    //    y = dist * Mathf.Sin((user.viewAngle / 2) * Mathf.Deg2Rad);
    //    Vector3 position = user.transform.position + user.transform.forward * x + user.transform.up * y;
    //    Handles.DrawLine(user.transform.position, position);

    //    x = dist * Mathf.Cos((-user.viewAngle / 2) * Mathf.Deg2Rad);
    //    y = dist * Mathf.Sin((-user.viewAngle / 2) * Mathf.Deg2Rad);
    //    Vector3 position2 = user.transform.position + user.transform.forward * x + user.transform.up * y;
    //    Handles.DrawLine(user.transform.position, position2);

    //    if(user.VisibleTargets != null && user.VisibleTargets.Count > 0)
    //    foreach (Transform visibleTarget in user.VisibleTargets)
    //    {
    //        Handles.DrawLine(user.tr.position, visibleTarget.position);
    //    }
    //}
}