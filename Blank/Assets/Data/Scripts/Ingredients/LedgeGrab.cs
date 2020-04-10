using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrab : MonoBehaviour
{
  //  public enum Orientation { nord, nordEst, est, sudEst, sud, sudOuest, ouest , nordOuest}
    //public Orientation ori;

    public enum LedgeGrabType { mur, plateforme}
    public LedgeGrabType type;
    [HideInInspector]
    public int currentOrientation = 0;

    //private void OnValidate()
    //{
    //    transform.eulerAngles = Vector3.zero;
    //    switch (ori)
    //    {
    //        case Orientation.nord:
    //            transform.eulerAngles = new Vector3(0f, 0f, 0f);
    //            currentOrientation = 0;
    //            break;
    //        case Orientation.nordEst:
    //            transform.eulerAngles = new Vector3(0f, 45f, 0f);
    //            currentOrientation = 1;
    //            break;
    //        case Orientation.est:
    //            transform.eulerAngles = new Vector3(0f, 90f, 0f);
    //            currentOrientation = 2;
    //            break;
    //        case Orientation.sudEst:
    //            transform.eulerAngles = new Vector3(0f, 135f, 0f);
    //            currentOrientation = 3;
    //            break;
    //        case Orientation.sud:
    //            transform.eulerAngles = new Vector3(0f, 180f, 0f);
    //            currentOrientation = 4;
    //            break;
    //        case Orientation.sudOuest:
    //            transform.eulerAngles = new Vector3(0f, 225f, 0f);
    //            currentOrientation = 5;
    //            break;
    //        case Orientation.ouest:
    //            transform.eulerAngles = new Vector3(0f, 270f, 0f);
    //            currentOrientation = 6;
    //            break;
    //        case Orientation.nordOuest:
    //            transform.eulerAngles = new Vector3(0f, 315f, 0f);
    //            currentOrientation = 7;
    //            break;
    //    }
    //}
    
}
